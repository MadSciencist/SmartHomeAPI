using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class NodeDataRepository : GenericRepository<NodeData>, INodeDataRepository
    {
        private readonly ILogger _logger;

        public NodeDataRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(NodeRepository));
        }

        public async Task<NodeData> AddSingleAsync(EDataRequestReason reason, NodeDataMagnitude data)
        {
            var nodeData = new NodeData
            {
                RequestReasonId = (int) reason,
                TimeStamp = DateTime.UtcNow,
                Magnitudes = new List<NodeDataMagnitude>
                {
                    data
                },
                NodeId = 1
            };

            await base.CreateAsync(nodeData);
            return nodeData;
        }

        public async Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitude> data)
        {
            var nodeData = new NodeData
            {
                RequestReasonId = (int)reason,
                TimeStamp = DateTime.UtcNow,
                Magnitudes = data,
                NodeId = 1
            };

            await base.CreateAsync(nodeData);
            return nodeData;
        }
    }
}
