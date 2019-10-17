using SmartHome.Core.Entities.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IPhysicalPropertyService
    {
        Task<PhysicalProperty> GetByMagnitudeAsync(string magnitude);
    }
}
