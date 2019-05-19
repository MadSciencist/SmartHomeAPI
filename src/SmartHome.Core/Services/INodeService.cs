using Newtonsoft.Json.Linq;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Utils;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface INodeService : IUserAuditable
    {
        Task<ServiceResult<NodeDto>> CreateNode(NodeDto nodeData);
        Task<ServiceResult<object>> Control(int nodeId, string operations, JObject commandParams);
        //IEnumerable<Command> GetNodeCommands(int nodeId);
    }
}