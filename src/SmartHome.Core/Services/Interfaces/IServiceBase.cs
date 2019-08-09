using SmartHome.Core.Domain.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface IServiceBase
    {
        ClaimsPrincipal Principal { get; set; }
        int GetCurrentUserId();
        Task<AppUser> GetCurrentUser();
    }
}