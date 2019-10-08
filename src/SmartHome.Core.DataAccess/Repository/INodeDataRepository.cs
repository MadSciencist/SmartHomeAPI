using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface INodeDataRepository : IGenericRepository<NodeData>
    {
        Task<NodeData> AddSingleAsync(int nodeId, int samplesToKeep, EDataRequestReason reason, NodeDataMagnitude data);

        Task<NodeData> AddManyAsync(int nodeId, int samplesToKeep, EDataRequestReason reason,
            ICollection<NodeDataMagnitude> data);
    }
}
