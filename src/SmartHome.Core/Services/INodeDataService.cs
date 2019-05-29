using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface INodeDataService : IServiceBase
    {
        Task<NodeData> AddSingleAsync(EDataRequestReason reason, NodeDataDto data);
        Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitude> data);
    }
}