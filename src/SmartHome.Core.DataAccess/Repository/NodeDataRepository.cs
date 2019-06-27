using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeDataRepository : GenericRepository<NodeData>, INodeDataRepository
    {
        public NodeDataRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
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
                RequestReasonId = (int) reason,
                TimeStamp = DateTime.UtcNow,
                Magnitudes = new List<NodeDataMagnitude>
                {
                    data
                },
                NodeId = nodeId
            };

            return await base.CreateAsync(nodeData);
        }
    }
}
