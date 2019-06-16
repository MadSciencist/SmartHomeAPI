using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface IStrategyRepository : IGenericRepository<ControlStrategy>
    {
        new Task<IEnumerable<ControlStrategy>> GetAll();
    }
}
