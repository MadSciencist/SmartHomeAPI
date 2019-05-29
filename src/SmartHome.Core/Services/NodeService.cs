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
    public class NodeService : ServiceBase, INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IValidator<NodeDto> _validator;
        private readonly AppDbContext _context;

        public NodeService(ILifetimeScope container, INodeRepository nodeRepository, IMapper mapper, IValidator<NodeDto> validator, AppDbContext context) : base(container, mapper)
        {
            _nodeRepository = nodeRepository;
            _validator = validator;
            _context = context;
        }

        public async Task<ServiceResult<NodeDto>> CreateNode(NodeDto nodeData)
        {
            var response = new ServiceResult<NodeDto>();
            var validationResult = _validator.Validate(nodeData);

            if (!validationResult.IsValid)
            {
                response.Alerts = validationResult.GetValidationMessages();
                return response;
            }

            var userId = GetCurrentUserId(Principal);
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
                    return response;
                }
            }
        }

        public async Task<ServiceResult<object>> Control(int nodeId, string command, JObject commandParams)
        {
            var response = new ServiceResult<object>();

            // get the node
            var node = await _nodeRepository.GetByIdAsync(nodeId);
            var userId = GetCurrentUserId(Principal);

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
