using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class StrategyRepository : GenericRepository<ControlStrategy>, IStrategyRepository
    {
        public StrategyRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public new async Task<IEnumerable<ControlStrategy>> GetAll()
        {
            return await Context.ControlStrategies
                .Include(x => x.ControlStrategyLinkages)
                .ToListAsync();
        }

        public override async Task<ControlStrategy> GetByIdAsync(int id)
        {
            return await Context.ControlStrategies
                .Include(x => x.ControlStrategyLinkages)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}