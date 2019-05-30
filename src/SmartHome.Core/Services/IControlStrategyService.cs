using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface IControlStrategyService : IServiceBase
    {
        Task<ServiceResult<CommandEntityDto>> CreateCommand(string alias, string executorClass);
        Task<ServiceResult<ControlStrategyDto>> AttachAvailableCommand(int strategyId, int commandId);
        Task<ServiceResult<ControlStrategyDto>> CreateStrategy(ControlStrategyDto input);
    }
}