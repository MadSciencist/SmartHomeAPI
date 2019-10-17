using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
{
    public interface INodeDataRepository : IGenericRepository<NodeData>
    {
        Task<NodeData> AddSingleAsync(NodeData data, int samplesToKeep);

        //Task<NodeData> AddManyAsync(int nodeId, int samplesToKeep, ICollection<NodeDataMagnitude> data);
    }
}
