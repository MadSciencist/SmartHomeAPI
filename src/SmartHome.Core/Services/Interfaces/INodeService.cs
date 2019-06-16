using Newtonsoft.Json.Linq;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface INodeService : IServiceBase
    {
        Task<ServiceResult<NodeDto>> CreateNode(NodeDto nodeData);
        Task<ServiceResult<object>> Control(int nodeId, string operations, JObject commandParams);
    }
}