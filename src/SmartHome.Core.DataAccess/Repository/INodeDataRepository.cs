using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface INodeDataRepository : IGenericRepository<NodeData>
    {
        Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitudes> data);
        Task<NodeData> AddSingleAsync(EDataRequestReason reason, NodeDataMagnitudes data);
    }
}
