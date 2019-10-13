using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface INodeDataRepository : IGenericRepository<NodeData>
    {
        Task<NodeData> AddSingleAsync(int nodeId, int samplesToKeep, NodeDataMagnitude data);

        Task<NodeData> AddManyAsync(int nodeId, int samplesToKeep, ICollection<NodeDataMagnitude> data);
    }
}
