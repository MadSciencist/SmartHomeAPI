using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Control;
using SmartHome.Core.Persistence;
using SmartHome.Core.Repository;
using SmartHome.Core.Utils;
using SmartHome.Domain.Entity;

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

        public IEnumerable<NodeCommand> GetNodeCommands(ClaimsPrincipal principal, int nodeId)
        {
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));

            //TODO check if user has permissions

            // TODO move this to repo
            var nodeCommands = _nodeRepository
                .AsQueryableNoTrack()
                .Include(x => x.AllowedCommands)
                .ThenInclude(x => x.NodeCommand)
                .FirstOrDefault(x => x.Id == nodeId)
                ?.AllowedCommands
                .Select(x => new NodeCommand
                {
                    Id = x.NodeCommand.Id,
                    BaseUri = x.NodeCommand.BaseUri,
                    Name = x.NodeCommand.Name,
                    Description = x.NodeCommand.Description,
                    Type = x.NodeCommand.Type,
                    Value = x.NodeCommand.Value
                    // we want to avoid nodes to get rid off circular dependencies 
                    // TODO create NodeCommandDTO without nodes property
                });

            return nodeCommands;
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

        public async Task<object> Control(ClaimsPrincipal principal, int nodeId, string command)
        {
            // get the node
            Node node = await _nodeRepository.GetByIdAsync(nodeId);
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));

            // check permissions
            if(node.AllowedUsers.Any(x => x.UserId != userId))
            {
                throw new InvalidOperationException("No access");
            }

            var commandEntity = node.AllowedCommands?.FirstOrDefault(x => x.NodeCommand?.Name == command);
            if (commandEntity == null)
            {
                throw new InvalidOperationException("Command not allowed");
            }

            // resolve control executor
            string controlStrategyExecutorClass = node.ControlStrategy.Strategy;
            var strategy = _container.ResolveNamed(controlStrategyExecutorClass, typeof(IControlStrategy)) as IControlStrategy;

            if (strategy == null)
            {
                throw new InvalidOperationException("Not existing strategy");
            }

            return await strategy.Execute(node, commandEntity.NodeCommand);
        }
    }
}
