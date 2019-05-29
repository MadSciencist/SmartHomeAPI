using System.Threading.Tasks;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.Services
{
    public interface IControlStrategyService : IServiceBase
    {
        Task<ServiceResult<ControlStrategyDto>> CreateStrategy(ControlStrategyDto input);
    }
}