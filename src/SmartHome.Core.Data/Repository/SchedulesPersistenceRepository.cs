using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.SchedulingEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class SchedulesPersistenceRepository : GenericRepository<ScheduleEntity>, ISchedulesPersistenceRepository
    {
        public SchedulesPersistenceRepository(ILifetimeScope container) : base(container)
        {
        }

        public override async Task<IEnumerable<ScheduleEntity>> GetAllAsync()
        {
            return await Context.SchedulesPersistence
                .Include(x => x.CreatedBy)
                .Include(x => x.JobType)
                .ToListAsync();
        }
    }
}
