using Autofac;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Core.Security;

namespace SmartHome.Core.Services
{
    public class NodeService : ServiceBase<NodeDto, object>, INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly NodeAuthorizationProvider _authProvider;

        public NodeService(ILifetimeScope container, INodeRepository nodeRepository, NodeAuthorizationProvider authorizationProvider) : base(container)
        {
            _nodeRepository = nodeRepository;
            _authProvider = authorizationProvider;
        }

        public async Task<ServiceResult<IEnumerable<NodeDto>>> GetAll()
        {
            var response = new ServiceResult<IEnumerable<NodeDto>>(Principal);

            try
            {
                var nodes = await _nodeRepository.GetAll();
                response.Data = Mapper.Map<IEnumerable<NodeDto>>(nodes);

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }
        
        public async Task<ServiceResult<NodeDto>> CreateNode(NodeDto nodeData)
        {
            var response = new ServiceResult<NodeDto>(Principal);
            var validationResult = Validator.Validate(nodeData);

            if (!validationResult.IsValid)
            {
                response.Alerts = validationResult.GetValidationMessages();
                return response;
            }

            if (!_authProvider.Authorize(null, Principal, OperationType.Add))
            {
                throw new SmartHomeUnauthorizedException(
                    $"User ${Principal.Identity.Name} is not authorized to add new node");
            }

            var userId = GetCurrentUserId();
            var nodeToCreate = Mapper.Map<Node>(nodeData);

            nodeToCreate.CreatedById = userId;
            nodeToCreate.Created = DateTime.UtcNow;

            return await SaveNode(nodeToCreate, userId, response);
        }

        private async Task<ServiceResult<NodeDto>> SaveNode(Node nodeToCreate, int userId, ServiceResult<NodeDto> response)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var createdNode = await _nodeRepository.CreateAsync(nodeToCreate);

                    // create entry in link table
                    DbContext.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode.Id,
                        UserId = userId
                    });

                    DbContext.SaveChanges();
                    transaction.Commit();
                    response.Data = Mapper.Map<NodeDto>(createdNode);
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
        }

        public async Task<ServiceResult<object>> Control(int nodeId, string command, JObject commandParams)
        {
            var response = new ServiceResult<object>(Principal);

            var node = await _nodeRepository.GetByIdAsync(nodeId);

            if (!_authProvider.Authorize(null, Principal, OperationType.Execute))
            {
                throw new SmartHomeUnauthorizedException($"User ${Principal.Identity.Name} is not authorized to add new node.");
            }
            
            try
            {
                // resolve control executor
                var executorFullyQualifiedName = node.ControlStrategy.ContractAssembly.Split(".dll")[0] + ".Commands." + command;

                if (!(Container.ResolveNamed<object>(executorFullyQualifiedName) is IControlStrategy strategy))
                {
                    response.Alerts.Add(new Alert($"{command} is not valid for strategy: {node.ControlStrategy.ContractAssembly}", MessageType.Error));
                    return response;
                }

                await strategy.Execute(node, commandParams);
                response.ResponseStatusCodeOverride = StatusCodes.Status202Accepted;
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
