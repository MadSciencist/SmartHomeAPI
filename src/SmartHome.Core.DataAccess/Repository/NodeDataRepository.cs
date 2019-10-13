using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeDataRepository : GenericRepository<NodeData>, INodeDataRepository
    {
        public NodeDataRepository(ILifetimeScope container) : base(container)
        {
        }

        public override IQueryable<NodeData> AsQueryableNoTrack()
        {
            return base.AsQueryableNoTrack().Include(x => x.Magnitudes);
        }

        public async Task<NodeData> AddSingleAsync(int nodeId, int samplesToKeep, EDataRequestReason reason, NodeDataMagnitude data)
        {
            var currentCount = await Context.NodeData.CountAsync(x =>
                x.NodeId == nodeId && x.Magnitudes.Any(m => m.Magnitude == data.Magnitude));

            if (currentCount > samplesToKeep) //keep only last x samples
            {
                var numToRemove = currentCount - samplesToKeep - 1; // -1 because we will add new record in next lines
                var toRemove = Context.NodeData.Where(s => s.NodeId == nodeId && s.Magnitudes.Any(x => x.Magnitude == data.Magnitude))
                    .OrderBy(s => s.TimeStamp)
                    .Take(numToRemove);

                Context.NodeData.RemoveRange(toRemove);
            }

            var nodeData = new NodeData
            {
                RequestReasonId = (int)reason,
                TimeStamp = DateTime.UtcNow,
                Magnitudes = new List<NodeDataMagnitude>
                {
                    data
                },
                NodeId = nodeId
            };

            return await base.CreateAsync(nodeData);
        }

        public async Task<NodeData> AddManyAsync(int nodeId, int samplesToKeep, EDataRequestReason reason, ICollection<NodeDataMagnitude> data)
        {
            foreach (var magnitude in data)
            {
                var currentCount = await Context.NodeData.CountAsync(x =>
                    x.NodeId == nodeId && x.Magnitudes.Any(m => m.Magnitude == magnitude.Magnitude));

                if (currentCount > samplesToKeep) //keep only last x samples
                {
                    var numToRemove = currentCount - samplesToKeep - 1; // -1 because we will add new record in next lines
                    var toRemove = Context.NodeData.Where(s => s.NodeId == nodeId && s.Magnitudes.Any(x => x.Magnitude == magnitude.Magnitude))
                        .OrderBy(s => s.TimeStamp)
                        .Take(numToRemove);

                    Context.NodeData.RemoveRange(toRemove);
                }
            }

            var nodeData = new NodeData
            {
                RequestReasonId = (int)reason,
                TimeStamp = DateTime.UtcNow,
                Magnitudes = data,
                NodeId = nodeId
            };

            return await base.CreateAsync(nodeData);
        }
    }
}
