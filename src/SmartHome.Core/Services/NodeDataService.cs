using System;
using Autofac;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NodeDataService : ServiceBase<object, object>, INodeDataService
    {
        private readonly INodeDataRepository _nodeDataRepository;

        public NodeDataService(ILifetimeScope container, INodeDataRepository nodeDataRepository) : base(container)
        {
            _nodeDataRepository = nodeDataRepository;
        }

        public async Task<ServiceResult<PaginatedList<NodeData>>> GetNodeData(int nodeId, int pageNumber, int pageSize)
        {
            var response = new ServiceResult<PaginatedList<NodeData>>(Principal);

            var queryable = _nodeDataRepository.AsQueryableNoTrack()
                .Where(x => x.NodeId == nodeId);

            var paginated = await PaginatedList<NodeData>.CreateAsync(queryable, pageNumber, pageSize);
            response.Data = paginated;

            return response;
        }

        public async Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason, NodeDataMagnitudeDto data)
        {
            return await _nodeDataRepository.AddSingleAsync(nodeId, reason, new NodeDataMagnitude
            {
                Magnitude = data.Magnitude,
                Unit = data.Unit,
                Value = data.Value
            });
        }
    }
}
