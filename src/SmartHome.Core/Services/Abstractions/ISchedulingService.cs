using System;
using System.Collections.Generic;
using SmartHome.Core.Dto;
using System.Threading.Tasks;
using SmartHome.Core.Entities.SchedulingEntity;
using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.Services.Abstractions
{
    public interface ISchedulingService : IServiceBase
    {
        Task<ServiceResult<IEnumerable<SchedulesPersistence>>> GetJobs();
        Task<ServiceResult<SchedulesPersistence>> AddNodeCommandJobAsync(ScheduleNodeCommandJobDto pram);
    }
}
