using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface INodeDataRepository : IGenericRepository<NodeData>
    {
        Task<NodeData> AddSingleAsync(int nodeId, int samplesToKeep, EDataRequestReason reason, NodeDataMagnitude data);
    }
}
