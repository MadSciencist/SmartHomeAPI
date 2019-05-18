using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.DataAccess;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Utils;

namespace SmartHome.Core.Services
{
    public class NodeService : INodeService
    {
        public ClaimsPrincipal ClaimsOwner { get; set; }

        private readonly INodeRepository _nodeRepository;
        private readonly IMapper _mapper;
        private readonly ILifetimeScope _container;
        private readonly AppDbContext _context;

        public NodeService(ILifetimeScope container, INodeRepository nodeRepository, IMapper mapper, AppDbContext context)
        {
            _container = container;
            _nodeRepository = nodeRepository;
            _mapper = mapper;
            _context = context;
        }

        public IEnumerable<Command> GetNodeCommands(int nodeId)
        {
            //int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));

            ////TODO check if user has permissions

            //// TODO move this to repo
            //var nodeCommands = _nodeRepository
            //    .AsQueryableNoTrack()
            //    .Include(x => x.AllowedCommands)
            //    .ThenInclude(x => x.NodeCommand)
            //    .FirstOrDefault(x => x.Id == nodeId)
            //    ?.AllowedCommands
            //    .Select(x => new NodeCommand
            //    {
            //        Id = x.NodeCommand.Id,
            //        BaseUri = x.NodeCommand.BaseUri,
            //        Name = x.NodeCommand.Name,
            //        Description = x.NodeCommand.Description,
            //        Type = x.NodeCommand.Type,
            //        Value = x.NodeCommand.Value
            //        // we want to avoid nodes to get rid off circular dependencies 
            //        // TODO create NodeCommandDTO without nodes property
            //    });

            //return nodeCommands;

            return new List<Command>();
        }

        public async Task<CreateNodeDto> CreateNode(CreateNodeDto nodeData)
        {
            var userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(ClaimsOwner));

            var nodeToCreate = _mapper.Map<Node>(nodeData);

            nodeToCreate.CreatedById = userId;
            nodeToCreate.Created = DateTime.UtcNow;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Node createdNode = await _nodeRepository.CreateAsync(nodeToCreate);

                    // create entry in link table - using Id's works fine
                    _context.Add(new AppUserNodeLink()
                    {
                        NodeId = createdNode.Id,
                        UserId = userId
                    });

                    _context.SaveChanges();
                    transaction.Commit();
                    return _mapper.Map<CreateNodeDto>(createdNode);
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<object> Control(int nodeId, string command, JObject commandParams)
        {
            // get the node
            Node node = await _nodeRepository.GetByIdAsync(nodeId);
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(ClaimsOwner));

            // check permissions
            if (node.AllowedUsers.Any(x => x.UserId != userId))
            {
                throw new SmartHomeException("No access");
            }

            var commandEntity = node.ControlStrategy?.AllowedCommands.FirstOrDefault(x => x.Command?.Alias?.ToLower() == command.ToLower());
            if (commandEntity == null)
            {
                throw new SmartHomeException("Command not allowed");
            }

            // resolve control executor
            var strategy = node.ControlStrategy;
            var executorFullyQualifiedName =
                $"SmartHome.Core.Contracts.{strategy.ProviderName}.Control.{strategy.ControlContext}.{commandEntity.Command.ExecutorClassName}";

            if (!(_container.ResolveNamed<object>(executorFullyQualifiedName) is IControlStrategy strategyExecutor))
            {
                throw new SmartHomeException("Not existing strategy");
            }

            return await strategyExecutor.Execute(node, commandEntity.Command, commandParams);
        }
    }
}
