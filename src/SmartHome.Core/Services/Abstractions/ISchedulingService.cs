using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.SchedulingEntity;
using SmartHome.Core.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Matty.Framework;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Services.Abstractions
{
    public interface ISchedulingService : IServiceBase
    {
        Task<ServiceResult<IEnumerable<ScheduleEntity>>> GetJobs();
        Task<ServiceResult<ScheduleEntity>> AddNodeCommandJobAsync(ScheduleNodeCommandJobDto pram);
        Task<ServiceResult<ScheduleEntity>> UpdateJobStatus(int id, JobStatus status);
        Task<ServiceResult<object>> RemoveJob(int id);
    }
}
