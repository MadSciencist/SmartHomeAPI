using Autofac;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.DataAccess;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.BusinessLogic
{
    public class NodeService : INodeService
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public string MyProperty { get; set; }

        private readonly INodeRepository _nodeRepository;
        private readonly ILifetimeScope _container;
        private readonly AppDbContext _context;

        public NodeService(INodeRepository nodeRepository, ILifetimeScope container, AppDbContext context)
        {
            _nodeRepository = nodeRepository;
            _container = container;
        }

        public IEnumerable<Command> GetNodeCommands(ClaimsPrincipal principal, int nodeId)
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

        public async Task<Node> CreateNode(ClaimsPrincipal principal, string name, string identifier, string description, string type)
        {
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));

            var node = new Node
            {
                Created = DateTime.Now,
                CreatedById = userId,
                Name = name,
                Description = description,
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Node createdNode = await _nodeRepository.CreateAsync(node);

                    // create entry in link table - using Id's works fine
                    _context.Add(new AppUserNodeLink()
                    {
                        NodeId = createdNode.Id,
                        UserId = userId
                    });

                    _context.SaveChanges();
                    transaction.Commit();
                    return createdNode;
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public async Task<object> Control(ClaimsPrincipal principal, int nodeId, string command, JObject commandParams)
        {
            // get the node
            Node node = await _nodeRepository.GetByIdAsync(nodeId);
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));

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
                $"SmartHome.Core.Providers.{strategy.ProviderName}.Contracts.{strategy.ContextName}.{commandEntity.Command.ExecutorClassName}";

            if (!(_container.ResolveNamed<object>(executorFullyQualifiedName) is IControlStrategy strategyExecutor))
            {
                throw new SmartHomeException("Not existing strategy");
            }

            return await strategyExecutor.Execute(node, commandEntity.Command, commandParams);
        }
    }
}
