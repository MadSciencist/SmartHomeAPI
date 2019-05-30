using System.Security.Claims;

namespace SmartHome.API.Security
{
    public interface ITrustProvider
    {
        bool IsTrustedRequest(ClaimsPrincipal principal);
    }
}
