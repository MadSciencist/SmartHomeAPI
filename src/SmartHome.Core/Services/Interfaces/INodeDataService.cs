using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface INodeDataService : IServiceBase
    {
        Task<NodeData> AddSingleAsync(int nodeId, EDataRequestReason reason, NodeDataMagnitudeDto data);
        Task<ServiceResult<PaginatedList<NodeData>>> GetNodeData(int nodeId, int pageNumber, int pageSize);
    }
}