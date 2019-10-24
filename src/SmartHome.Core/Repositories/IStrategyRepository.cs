using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Repositories
{
    public interface IControlStrategyRepository : ITransactionalRepository<ControlStrategy, int>
    {
        Task<ControlStrategy> CreateWithPropertyLinksAsync(ControlStrategy strategy, IEnumerable<int> physicalPropertyIds);

        Task<ControlStrategy> UpdateWithLinksAsync(ControlStrategy entity, IEnumerable<PhysicalPropertyControlStrategyLink> links);
    }
}
