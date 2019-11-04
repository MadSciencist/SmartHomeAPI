using Autofac;
using Matty.Framework;
using Matty.Framework.Abstractions;
using Matty.Framework.Enums;
using Matty.Framework.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema.Generation;
using SmartHome.Core.Control;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Attributes;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Infrastructure.Validators;
using SmartHome.Core.Repositories;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NodeService : CrudServiceBase<NodeDto, EntityBase>, INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IAuthorizationProvider<Node> _authProvider;
        private readonly IAppUserNodeLinkRepository _appUserNodeLinkRepository;
        private readonly IPhysicalPropertyService _physicalPropertyService;

        public NodeService(ILifetimeScope container,
            INodeRepository nodeRepository,
            IAuthorizationProvider<Node> authorizationProvider,
            IAppUserNodeLinkRepository appUserNodeLinkRepository,
            IPhysicalPropertyService physicalPropertyService) : base(container)
        {
            _nodeRepository = nodeRepository;
            _authProvider = authorizationProvider;
            _appUserNodeLinkRepository = appUserNodeLinkRepository;
            _physicalPropertyService = physicalPropertyService;
        }

        public async Task<ServiceResult<IEnumerable<NodeDto>>> GetAll()
        {
            var response = new ServiceResult<IEnumerable<NodeDto>>(Principal);

            try
            {
                var nodes = await _nodeRepository.GetAllAsync();
                response.Data = Mapper.Map<IEnumerable<NodeDto>>(nodes);

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        public async Task<ServiceResult<NodeDto>> CreateNode(NodeDto nodeDto)
        {
            var response = new ServiceResult<NodeDto>(Principal);
            var validationResult = Validator.Validate(nodeDto);

            if (!validationResult.IsValid)
            {
                response.AddAlerts(validationResult.GetValidationMessages());
                return response;
            }

            if (!_authProvider.Authorize(null, Principal, OperationType.Add))
            {
                throw new SmartHomeUnauthorizedException(
                    $"User ${Principal.Identity.Name} is not authorized to add new node");
            }

            var userId = GetCurrentUserId();
            var nodeToCreate = Mapper.Map<Node>(nodeDto);

            return await SaveNode(nodeToCreate, userId, response);
        }
        
        private async Task<ServiceResult<NodeDto>> SaveNode(Node nodeToCreate, int userId, ServiceResult<NodeDto> response)
        {
            using var transaction = _nodeRepository.BeginTransaction();
            try
            {
                var createdNode = await _nodeRepository.CreateAsync(nodeToCreate);

                // create entry in link table
                await _appUserNodeLinkRepository.CreateAsync(new AppUserNodeLink
                {
                    NodeId = createdNode.Id,
                    UserId = userId
                });

                transaction.Commit();
                response.Data = Mapper.Map<NodeDto>(createdNode);
                response.Data.CreatedById = userId;
                response.Alerts.Add(new Alert("Successfully created", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        public async Task<ServiceResult<object>> ExecuteCommand(int nodeId, string command, JObject commandParams)
        {
            if (string.IsNullOrEmpty(command)) throw new ArgumentException(nameof(command));

            var response = new ServiceResult<object>(Principal);

            var node = await _nodeRepository.GetByIdAsync(nodeId);

            if (node is null)
                throw new SmartHomeEntityNotFoundException($"Node with id {nodeId} doesn't exists.");

            if (!_authProvider.Authorize(node, Principal, OperationType.Execute))
                throw new SmartHomeUnauthorizedException($"User ${Principal.Identity.Name} is not authorized to execute command.");

            try
            {
                // resolve control executor - convention is SmartHome.Core.Contracts.{name}.Commands.CommandClass
                var executorFullyQualifiedName = node.ControlStrategy.GetCommandFullyQuallifiedName(command);

                if (!Container.IsRegisteredWithName<object>(executorFullyQualifiedName))
                {
                    response.Alerts.Add(new Alert($"Given command is invalid in this context: {command}",
                        MessageType.Error));
                    return response;
                }

                if (Container.ResolveNamed<object>(executorFullyQualifiedName, new NamedParameter("node", node)) is
                    IControlCommand executor)
                    await executor.Execute(commandParams);

                response.ResponseStatusCodeOverride = StatusCodes.Status202Accepted;
                return response;
            }
            catch (SmartHomeNodeOfflineException)
            {
                if (Principal.Identity.Name != "system") throw;
                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        public async Task<ServiceResult<object>> GetCommandParamSchema(int nodeId, string command)
        {
            if (string.IsNullOrEmpty(command)) throw new ArgumentException(nameof(command));

            var response = new ServiceResult<object>(Principal);

            var node = await _nodeRepository.GetByIdAsync(nodeId);

            if (node is null)
                throw new SmartHomeEntityNotFoundException($"Node with id {nodeId} doesn't exists.");

            if (!_authProvider.Authorize(null, Principal, OperationType.Read))
                throw new SmartHomeUnauthorizedException($"User ${Principal.Identity.Name} is not authorized to add new node.");

            try
            {
                // resolve control executor - convention is SmartHome.Core.Contracts.{name}.Commands.CommandClass
                var executorFullyQualifiedName = node.ControlStrategy.GetCommandFullyQuallifiedName(command);

                if (!Container.IsRegisteredWithName<object>(executorFullyQualifiedName))
                {
                    response.Alerts.Add(new Alert($"Given command is invalid in this context: {command}", MessageType.Error));
                    return response;
                }

                var executor = Container.ResolveNamed<object>(executorFullyQualifiedName, new NamedParameter("node", node)) as IControlCommand;

                var attr = executor?.GetType().GetAttribute<ParameterTypeAttribute>();
                var generator = new JSchemaGenerator();
                var schema = generator.Generate(attr?.ParameterType);
                response.Data = schema;

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }
    }
}
