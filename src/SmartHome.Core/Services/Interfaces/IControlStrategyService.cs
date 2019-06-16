using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface IControlStrategyService : IServiceBase
    {
        Task<ServiceResult<IEnumerable<ControlStrategyDto>>> GetAll();
        Task<ServiceResult<ControlStrategyDto>> CreateStrategy(ControlStrategyDto dto);
    }
}