using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeDataRepository : GenericRepository<NodeData>, INodeDataRepository
    {
        private readonly ILogger _logger;

        public NodeDataRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(NodeDataRepository));
        }

        public override IQueryable<NodeData> AsQueryableNoTrack()
        {
            return base.AsQueryableNoTrack().Include(x => x.Magnitudes);
        }

        public async Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason, NodeDataMagnitude data)
        {
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

            await base.CreateAsync(nodeData);
            return nodeData;
        }

        //public async Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitude> data)
        //{
        //    var nodeData = new NodeData
        //    {
        //        RequestReasonId = (int)reason,
        //        TimeStamp = DateTime.UtcNow,
        //        Magnitudes = data,
        //        NodeId = 1
        //    };

        //    await base.CreateAsync(nodeData);
        //    return nodeData;
        //}
    }
}
