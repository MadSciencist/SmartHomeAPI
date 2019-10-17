using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
{
    public class NodeDataRepository : GenericRepository<NodeData>, INodeDataRepository
    {
        public NodeDataRepository(ILifetimeScope container) : base(container)
        {
        }

        public async Task<NodeData> AddSingleAsync(NodeData data, int samplesToKeep)
        {
            var currentCount = await Context.NodeData
                .AsNoTracking()
                .CountAsync(x => x.NodeId == data.NodeId && x.Magnitude == data.Magnitude);

            if (currentCount >= samplesToKeep) //keep only last x samples
            {
                var numToRemove = currentCount - samplesToKeep + 1; // 1 because we will add new record in next lines
                var toRemove = Context.NodeData
                    .Where(x => x.NodeId == data.NodeId && x.Magnitude == data.Magnitude)
                    .OrderBy(s => s.Id)
                    .Take(numToRemove)
                    .AsNoTracking();

                Context.NodeData.RemoveRange(toRemove);
            }

            return await base.CreateAsync(data);
        }

        //public async Task<NodeData> AddManyAsync(int nodeId, int samplesToKeep, ICollection<NodeDataMagnitude> data)
        //{
        //    foreach (var magnitude in data)
        //    {
        //        int currentCount = await Context.NodeData
        //            .AsNoTracking()
        //            .CountAsync(x => x.NodeId == nodeId && x.Magnitudes.Any(m => m.Magnitude == magnitude.Magnitude));

        //        if (currentCount > samplesToKeep) //keep only last x samples
        //        {
        //            int numToRemove = currentCount - samplesToKeep - 1; // -1 because we will add new record in next lines
        //            IQueryable<NodeData> toRemove = Context.NodeData
        //                .AsNoTracking()
        //                .Where(s => s.NodeId == nodeId && s.Magnitudes.Any(x => x.Magnitude == magnitude.Magnitude))
        //                .OrderBy(s => s.Id)
        //                .Take(numToRemove);

        //            Context.NodeData.RemoveRange(toRemove);
        //        }
        //    }

        //    var nodeData = new NodeData
        //    {
        //        TimeStamp = DateTime.UtcNow,
        //        Magnitudes = data,
        //        NodeId = nodeId
        //    };

        //    return await base.CreateAsync(nodeData);
        //}
    }
}
