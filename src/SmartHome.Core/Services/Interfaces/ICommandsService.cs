using System.Threading.Tasks;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.Services
{
    public interface ICommandService : IServiceBase
    {
        Task<ServiceResult<CommandEntityDto>> CreateCommand(string alias, string executorClass);
    }
}