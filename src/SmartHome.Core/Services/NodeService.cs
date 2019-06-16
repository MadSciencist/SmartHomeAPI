using Autofac;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Authorization;
using SmartHome.Core.Control;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NodeService : ServiceBase<NodeDto, object>, INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IGenericRepository<ControlStrategy> _strategyRepository;
        private readonly NodeAuthorizationProvider _authProvider;

        public NodeService(ILifetimeScope container, INodeRepository nodeRepository,
            IGenericRepository<ControlStrategy> strategyRepository, NodeAuthorizationProvider authorizationProvider) : base(container)
        {
            _nodeRepository = nodeRepository;
            _strategyRepository = strategyRepository;
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

            // get the node
            var node = await _nodeRepository.GetByIdAsync(nodeId);
            var userId = GetCurrentUserId();

            // check permissions
            if (node.AllowedUsers.Any(x => x.UserId != userId))
            {
                response.Alerts.Add(new Alert("Permissions error", MessageType.Error));
                return response;
            }

            // TODO
            var commandEntity = node.ControlStrategy?.AllowedCommands.FirstOrDefault(x => x.Command?.Alias?.ToLower() == command.ToLower());
            if (commandEntity == null)
            {
                response.Alerts.Add(new Alert("Command not allowed", MessageType.Error));
                return response;
            }

            // resolve control executor
            var strategy = node.ControlStrategy;
            var executorFullyQualifiedName =
                $"SmartHome.Core.Contracts.{strategy.ControlProviderName}.Control.{strategy.ControlContext}.{commandEntity.Command.ExecutorClassName}";

            if (!(Container.ResolveNamed<object>(executorFullyQualifiedName) is IControlStrategy strategyExecutor))
            {
                response.Alerts.Add(new Alert("Not existing control strategy", MessageType.Error));
                return response;
            }

            var executionResult =  await strategyExecutor.Execute(node, commandEntity.Command, commandParams);
            response.Data = executionResult;

            return response;
        }
    }
}
