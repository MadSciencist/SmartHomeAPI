using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using System.Linq;

namespace SmartHome.Core.DataAccess.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy>, IStrategyRepository
    {
        public StrategyRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public override IQueryable<ControlStrategy> AsQueryableNoTrack()
        {
            return base.AsQueryableNoTrack().Include(x => x.RegisteredMagnitudes);
        }
    }
}