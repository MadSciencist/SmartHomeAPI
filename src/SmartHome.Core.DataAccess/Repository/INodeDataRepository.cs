using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
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
