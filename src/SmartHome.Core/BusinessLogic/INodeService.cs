using System.Security.Claims;
using System.Threading.Tasks;
using SmartHome.Core.Control;
using SmartHome.Domain.Entity;

namespace SmartHome.Core.Services
{
    public interface INodeService
    {
        Task<Node> CreateNode(ClaimsPrincipal principal, string name, string identifier, string description, string type);
        Task<object> Control(int nodeId, ControlCommand operations);
    }
}