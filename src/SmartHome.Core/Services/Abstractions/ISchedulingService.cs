using System;
using SmartHome.Core.Dto;
using System.Threading.Tasks;
using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.Services.Abstractions
{
    public interface ISchedulingService : IServiceBase
    {
        Task<ServiceResult<DateTimeOffset>> AddRepeatableJobAsync(ExecuteNodeCommandJobDto pram);
    }
}
