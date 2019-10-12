using Newtonsoft.Json.Linq;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface INodeService : IServiceBase
    {
        Task<ServiceResult<IEnumerable<NodeDto>>> GetAll();
        Task<ServiceResult<NodeDto>> CreateNode(NodeDto nodeData);
        Task<ServiceResult<object>> ExecuteCommand(int nodeId, string operations, JObject commandParams);
        Task<ServiceResult<object>> GetCommandParamSchema(int nodeId, string command);
    }
}