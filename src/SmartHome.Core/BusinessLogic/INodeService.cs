using Newtonsoft.Json.Linq;
using SmartHome.Domain.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.Core.BusinessLogic
{
    public interface INodeService
    {
        Task<Node> CreateNode(ClaimsPrincipal principal, string name, string identifier, string description, string type);
        Task<object> Control(ClaimsPrincipal principal, int nodeId, string operations, JObject commandParams);
        IEnumerable<Command> GetNodeCommands(ClaimsPrincipal principal, int nodeId);
        ClaimsPrincipal ClaimsPrincipal { get; set; }
        string MyProperty { get; set; }
    }
}