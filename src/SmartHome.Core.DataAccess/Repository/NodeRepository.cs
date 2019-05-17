using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeRepository : GenericRepository<Node>, INodeRepository
    {
        private readonly ILogger _logger;

        public NodeRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(NodeRepository));
        }

        public override async Task<Node> GetByIdAsync(int id)
        {
            return await _context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.AllowedCommands)
                        .ThenInclude(x => x.Command)
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

            return await _context.Nodes
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.AllowedCommands)
                        .ThenInclude(x => x.Command)
                .Include(x => x.ControlStrategy)
                    .ThenInclude(x => x.RegisteredSensors)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .FirstOrDefaultAsync(x => x.ClientId == clientId);
        }
    }
}