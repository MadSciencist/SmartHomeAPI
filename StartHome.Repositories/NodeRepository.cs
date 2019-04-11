using Microsoft.Extensions.Logging;
using SmartHome.Domain.Entity;

namespace SmartHome.Repositories
{
    public class NodeRepository : GenericRepository<Node>, INodeRepository
    {
        private readonly ILogger _logger;

        public NodeRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(NodeRepository));
        }
    }
}
