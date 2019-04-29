using Autofac;
using SmartHome.Core.BusinessLogic;
using SmartHome.Core.Control;
using SmartHome.Core.Persistence;
using SmartHome.Core.Repository;
using SmartHome.Core.Utils;
using SmartHome.Domain.Entity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly ILifetimeScope _container;
        private readonly AppDbContext _context;

        public NodeService(INodeRepository nodeRepository, ILifetimeScope container, AppDbContext context)
        {
            _nodeRepository = nodeRepository;
            _container = container;
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
                    _context.Add(new AppUserNode()
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

        public async Task<object> Control(int nodeId, ControlCommand command)
        {
            // get the node
            Node node = await _nodeRepository.GetByIdAsync(nodeId);
            string controlStrategyExecutorClass = node.ControlStrategy.Strategy;

            // resolve control executor
            var strategy = _container.ResolveNamed(controlStrategyExecutorClass, typeof(IControlStrategy)) as IControlStrategy;

            return await strategy.Execute(node, command);
        }
    }
}
