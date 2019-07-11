using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.DataAccess.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy>, IStrategyRepository
    {
        public StrategyRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
    }
}