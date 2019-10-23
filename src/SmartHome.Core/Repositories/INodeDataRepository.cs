using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Repositories
{
    public interface INodeDataRepository : ITransactionalRepository<NodeData, int>
    {
        Task<NodeData> AddSingleAsync(NodeData data, int samplesToKeep);
        Task AddManyAsync(int nodeId, int samplesToKeep, IEnumerable<NodeData> dataPlural);
        Task<IEnumerable<NodeData>> GetByMagnitudeAsync(int nodeId, string magnitude, int limit);
    }
}
