using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SmartHome.Core.Control;
using SmartHome.Domain.Entity;

namespace SmartHome.Core.BusinessLogic
{
    public interface INodeService
    {
        Task<Node> CreateNode(ClaimsPrincipal principal, string name, string identifier, string description, string type);
        Task<object> Control(ClaimsPrincipal principal, int nodeId, ControlCommand operations);
        IEnumerable<NodeCommand> GetNodeCommands(ClaimsPrincipal principal, int nodeId);
        ClaimsPrincipal ClaimsPrincipal { get; set; }
        string MyProperty { get; set; }
    }
}