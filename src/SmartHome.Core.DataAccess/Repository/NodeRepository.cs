using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeRepository : GenericRepository<Node>, INodeRepository
    {
        public NodeRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public new async Task<IEnumerable<Node>> GetAll()
        {
            return await Context.Nodes
                .Include(x => x.ControlStrategy)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .ToListAsync();
        }

        public override async Task<Node> GetByIdAsync(int id)
        {
            return await Context.Nodes
                .Include(x => x.ControlStrategy)
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
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .FirstOrDefaultAsync(x => x.ClientId == clientId);
        }
    }
}