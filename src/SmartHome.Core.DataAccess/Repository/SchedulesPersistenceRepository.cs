using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Entities.SchedulingEntity;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Core.DataAccess.Repository
{
    public class SchedulesPersistenceRepository : GenericRepository<SchedulesPersistence>, ISchedulesPersistenceRepository
    {
        public SchedulesPersistenceRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public override IEnumerable<SchedulesPersistence> GetAll()
        {
            return Context.SchedulesPersistence
                .Include(x => x.CreatedBy)
                .Include(x => x.JobType)
                .ToList();
        }
    }
}
