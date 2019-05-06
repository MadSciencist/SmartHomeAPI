using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Persistence;
using SmartHome.Domain.Entity;

namespace SmartHome.Core.Repository
{
    public class NodeRepository : GenericRepository<Node>, INodeRepository
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _context;

        public NodeRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(NodeRepository));
            _context = context;
        }

        public override async Task<Node> GetByIdAsync(int id)
        {
            return await _context.Nodes
                .Include(x => x.ControlStrategy)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .Include(x => x.AllowedCommands)
                    .ThenInclude(x => x.NodeCommand)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
