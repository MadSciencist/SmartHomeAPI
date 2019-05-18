using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;
using SmartHome.Core.Utils;

namespace SmartHome.Core.Services
{
    public interface INodeService : IUserAuditable
    {
        Task<CreateNodeDto> CreateNode(CreateNodeDto nodeData);
        Task<object> Control(int nodeId, string operations, JObject commandParams);
        IEnumerable<Command> GetNodeCommands(int nodeId);
    }
}