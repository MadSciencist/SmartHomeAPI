using Matty.Framework;
using SmartHome.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IPhysicalPropertyService : IServiceBase
    {
        /// <summary>
        /// Gets all possible physical property.
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<PhysicalPropertyDto>>> GetAll();

        /// <summary>
        /// Gets physical properties filtered by magnitude value.
        /// </summary>
        /// <param name="magnitudes"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<PhysicalPropertyDto>>> GetFilteredByMagnitudes(IEnumerable<string> magnitudes);

        /// <summary>
        /// Gets single physical property by it's magnitude.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        Task<ServiceResult<PhysicalPropertyDto>> GetByMagnitudeAsync(string magnitude);
    }
}
