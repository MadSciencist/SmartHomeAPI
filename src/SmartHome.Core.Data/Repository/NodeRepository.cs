using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeRepository : GenericRepository<Node>, INodeRepository
    {
        public NodeRepository(ILifetimeScope container) : base(container)
        {
        }

        public new async Task<IEnumerable<Node>> GetAll()
        {
            return await Context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.RegisteredMagnitudes)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllClientIdsAsync()
        {
            return await Context.Nodes
                .Select(x => x.ClientId)
                .ToListAsync();
        }

        public override async Task<Node> GetByIdAsync(int id)
        {
            return await Context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.RegisteredMagnitudes)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Node> GetByClientIdAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return null;
            }

            return await Context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.RegisteredMagnitudes)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .FirstOrDefaultAsync(x => x.ClientId == clientId);
        }
    }
}