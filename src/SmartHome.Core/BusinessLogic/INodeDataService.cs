using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Dto;

namespace SmartHome.Core.BusinessLogic
{
    public interface INodeDataService : IUserAuditable
    {
        Task<NodeData> AddSingleAsync(EDataRequestReason reason, NodeDataDto data);
        Task<NodeData> AddManyAsync(EDataRequestReason reason, ICollection<NodeDataMagnitude> data);
    }
}