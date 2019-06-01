using Autofac;
using AutoMapper;
using FluentValidation;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.DataAccess;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NodeService : ServiceBase<NodeDto, object>, INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IGenericRepository<ControlStrategy> _strategyRepository;
        private readonly AppDbContext _context;

        public NodeService(ILifetimeScope container, INodeRepository nodeRepository, IMapper mapper, IGenericRepository<ControlStrategy> strategyRepository,
            IValidator<NodeDto> validator, AppDbContext context) : base(container, mapper, validator)
        {
            _nodeRepository = nodeRepository;
            _strategyRepository = strategyRepository;
            _context = context;
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var createdNode = await _nodeRepository.CreateAsync(nodeToCreate);

                    // create entry in link table
                    _context.Add(new AppUserNodeLink
                    {
                        NodeId = createdNode.Id,
                        UserId = userId
                    });

                    _context.SaveChanges();
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

        public async Task<ServiceResult<NodeDto>> AttachControlStrategy(int nodeId, int strategyId)
        {
            var response = new ServiceResult<NodeDto>(Principal);

            var node = await _nodeRepository.GetByIdAsync(nodeId);
            if (node == null) throw new SmartHomeEntityNotFoundException($"Cannot find node with given Id: {nodeId}");

            // TODO auth

            var strategy = await _strategyRepository.GetByIdAsync(strategyId);
            if (strategy == null) throw new SmartHomeEntityNotFoundException($"Cannot find strategy with given Id: {strategyId}");

            try
            {
                node.ControlStrategyId = strategyId;
                var updated = await _nodeRepository.UpdateAsync(node);
                response.Data = Mapper.Map<NodeDto>(updated);
                response.Alerts.Add(new Alert("Successfully attached strategy", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
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
