using Matty.Framework;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface ISchedulingService : IServiceBase
    {
        Task<ServiceResult<JobScheduleDto>> GeJobById(int id);
        Task<ServiceResult<IEnumerable<JobScheduleDto>>> GetJobs();
        Task<ServiceResult<JobScheduleDto>> AddNodeCommandJobAsync(ScheduleNodeCommandJobDto pram);
        Task<ServiceResult<JobScheduleDto>> UpdateJobStatus(int id, JobStatus status);
        Task<ServiceResult<object>> RemoveJob(int id);
    }
}
