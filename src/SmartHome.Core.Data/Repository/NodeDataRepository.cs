using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
{
    public class NodeDataRepository : GenericRepository<NodeData, int>, INodeDataRepository
    {
        public NodeDataRepository(ILifetimeScope container) : base(container)
        {
        }

        public async Task<NodeData> AddSingleAsync(NodeData data, int samplesToKeep)
        {
            var currentCount = await Context.NodeData
                .AsNoTracking()
                .CountAsync(x => x.NodeId == data.NodeId && x.PhysicalProperty.Magnitude == data.PhysicalProperty.Magnitude);

            if (currentCount >= samplesToKeep) //keep only last x samples
            {
                var numToRemove = currentCount - samplesToKeep + 1; // 1 because we will add new record in next lines
                var toRemove = Context.NodeData
                    .Where(x => x.NodeId == data.NodeId && x.PhysicalProperty.Magnitude == data.PhysicalProperty.Magnitude)
                    .OrderBy(s => s.Id)
                    .Take(numToRemove)
                    .AsNoTracking();

                Context.NodeData.RemoveRange(toRemove);
            }

            // without that EF tries to insert this property again
            data.PhysicalProperty = null;

            return await base.CreateAsync(data);
        }

        public async Task AddManyAsync(int nodeId, int samplesToKeep, IEnumerable<NodeData> dataPlural)
        {
            // TODO refactor to delete with single fire
            foreach (var data in dataPlural)
            {
                var currentCount = await Context.NodeData
                    .AsNoTracking()
                    .CountAsync(x => x.NodeId == data.NodeId && x.PhysicalProperty.Magnitude == data.PhysicalProperty.Magnitude);

                if (currentCount >= samplesToKeep) //keep only last x samples
                {
                    var numToRemove = currentCount - samplesToKeep + 1; // 1 because we will add new record in next lines
                    var toRemove = Context.NodeData
                        .Where(x => x.NodeId == data.NodeId && x.PhysicalProperty.Magnitude == data.PhysicalProperty.Magnitude)
                        .OrderBy(s => s.Id)
                        .Take(numToRemove)
                        .AsNoTracking();

                    Context.NodeData.RemoveRange(toRemove);
                }
            }

            var toAdd = dataPlural.ToList().Select(x => new NodeData
            {
                Node = x.Node,
                PhysicalPropertyId = x.PhysicalPropertyId,
                TimeStamp = x.TimeStamp,
                Value = x.Value,
                NodeId = x.NodeId
            });

            await Context.NodeData.AddRangeAsync(toAdd);
            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NodeData>> GetByMagnitudeAsync(int nodeId, string magnitude, int limit)
        {
            return await Context.NodeData
                .Where(x => x.PhysicalProperty.Magnitude == magnitude && x.NodeId == nodeId)
                .OrderByDescending(x => x.TimeStamp)
                .Take(limit)
                .ToListAsync();
        }
    }
}
