using System.Security.Claims;
using System.Threading.Tasks;
using SmartHome.Domain.Entity;

namespace SmartHome.API.Services.Crud
{
    public interface ICrudNodeService
    {
        Task<Node> CreateNode(ClaimsPrincipal principal, string name, string identifier, string description, string type);
    }
}