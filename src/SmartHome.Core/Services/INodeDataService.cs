using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Utils;

namespace SmartHome.Core.Services
{
    public interface INodeDataService : IUserAuditable
    {
        Task<NodeData> AddSingleAsync(EDataRequestReason reason, NodeDataDto data);
        Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitude> data);
    }
}