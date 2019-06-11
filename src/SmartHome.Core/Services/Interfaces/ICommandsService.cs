using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface ICommandService : IServiceBase
    {
        Task<ServiceResult<IEnumerable<CommandEntityDto>>> GetCommands();
        Task<ServiceResult<CommandEntityDto>> CreateCommand(string alias, string executorClass);
    }
}