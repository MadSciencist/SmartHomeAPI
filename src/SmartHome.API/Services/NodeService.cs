using SmartHome.API.Persistence;
using SmartHome.API.Security.Utils;
using SmartHome.Domain.Entity;
using SmartHome.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.API.Services
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly AppIdentityDbContext _context;
        private readonly IGenericRepository<NodeType> _nodeTypeRepo;

        public NodeService(INodeRepository nodeRepository, IGenericRepository<NodeType> nodeTypeRepo, AppIdentityDbContext context)
        {
            _nodeRepository = nodeRepository;
            _nodeTypeRepo = nodeTypeRepo;
            _context = context;
        }

        public async Task<Node> CreateNode(ClaimsPrincipal principal, string name, string identifier, string description, string type)
        {
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));
            NodeType nodeType = _nodeTypeRepo.Find(x => x.Name == type).First();

            var node = new Node
            {
                Identifier = identifier,
                Created = DateTime.Now,
                CreatedById = userId,
                Type = nodeType,
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
    }
}
