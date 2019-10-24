using Matty.Framework;
using SmartHome.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IControlStrategyService : IServiceBase
    {
        /// <summary>
        /// Lists all strategies.
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<ControlStrategyDto>>> GetAll();

        /// <summary>
        /// Creates new strategy.
        /// </summary>
        /// <param name="dto">Parameters of strategy to create.</param>
        /// <returns>Created strategy.</returns>
        Task<ServiceResult<ControlStrategyDto>> Create(ControlStrategyDto dto);

        Task<ServiceResult<ControlStrategyDto>> Update(int id, ControlStrategyDto dto);
        Task<ServiceResult<object>> Delete(int id);
    }
}
