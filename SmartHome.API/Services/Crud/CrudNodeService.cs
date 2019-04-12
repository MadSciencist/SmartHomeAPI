using Microsoft.EntityFrameworkCore;
using SmartHome.API.Persistence.Identity;
using SmartHome.Domain.Entity;
using SmartHome.Domain.User;
using SmartHome.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SmartHome.API.Security.Utils;

namespace SmartHome.API.Services.Crud
{
    public class CrudNodeService : API.Services.Crud.ICrudNodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IGenericRepository<NodeType> _nodeTypeRepo;
        
        public CrudNodeService(INodeRepository nodeRepository, AppIdentityDbContext identityDbContext, IGenericRepository<NodeType> nodeTypeRepo)
        {
            _nodeRepository = nodeRepository;
            _identityDbContext = identityDbContext;
            _nodeTypeRepo = nodeTypeRepo;
        }

        public async Task<Node> CreateNode(ClaimsPrincipal principal, string name, string identifier, string description, string type)
        {
            int userId = Convert.ToInt32(ClaimsPrincipalHelper.GetClaimedIdentifier(principal));
            AppUser user = await _identityDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            NodeType nodeType = _nodeTypeRepo.Find(x => x.Name == type).First();

            var node = new Node()
            {
                Identifier = identifier,
                Created = DateTime.Now,
                CreatedBy = user,
                Type = nodeType,
                Name = name,
                Description = description,
            };

            return await _nodeRepository.CreateAsync(node);
        }
    }
}
