using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Domain.Entity;

namespace SmartHome.Core.DataAccess.Repository
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
                    .ThenInclude(x => x.AllowedCommands)
                        .ThenInclude(x => x.Command)
                .Include(x => x.CreatedBy)
                .Include(x => x.AllowedUsers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}