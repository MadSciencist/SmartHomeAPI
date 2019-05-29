using System.Security.Claims;

namespace SmartHome.Core.Services
{
    public interface IServiceBase
    {
        ClaimsPrincipal Principal { get; set; }
        int GetCurrentUserId(ClaimsPrincipal principal);
    }
}