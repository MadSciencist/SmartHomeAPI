using Matty.Framework;
using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IControlStrategyService : IServiceBase
    {
        Task<ServiceResult<IEnumerable<ControlStrategy>>> GetAll();
    }
}
