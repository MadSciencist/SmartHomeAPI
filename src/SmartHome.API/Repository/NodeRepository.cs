using Microsoft.Extensions.Logging;
using SmartHome.API.Persistence;
using SmartHome.Domain.Entity;
using SmartHome.Repositories;

namespace SmartHome.API.Repository
{
    public class NodeRepository : GenericRepository<Node>, INodeRepository
    {
        private readonly ILogger _logger;

        public NodeRepository(AppIdentityDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(NodeRepository));
        }
    }
}
