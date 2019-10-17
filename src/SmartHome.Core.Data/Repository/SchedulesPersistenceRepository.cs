using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.SchedulingEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
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
                .Include(x => x.UpdatedBy)
                .Include(x => x.JobStatusEntity)
                .Include(x => x.JobType)
                .Include(x => x.ScheduleType)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public override async Task<ScheduleEntity> GetByIdAsync(int id)
        {
            return await Context.SchedulesPersistence
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.JobStatusEntity)
                .Include(x => x.JobType)
                .Include(x => x.ScheduleType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<ScheduleEntity> GetFiltered(Expression<Func<ScheduleEntity, bool>> predicate)
        {
            return await Context.SchedulesPersistence
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.JobStatusEntity)
                .Include(x => x.JobType)
                .Include(x => x.ScheduleType)
                .FirstOrDefaultAsync(predicate);
        }

        public override async Task<IEnumerable<ScheduleEntity>> GetManyFiltered(Expression<Func<ScheduleEntity, bool>> predicate)
        {
            return await Context.SchedulesPersistence
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.JobStatusEntity)
                .Include(x => x.JobType)
                .Include(x => x.ScheduleType)
                .Where(predicate)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }
}
