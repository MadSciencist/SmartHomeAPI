using System.Security.Claims;

namespace SmartHome.API.Security
{
    public class AdminTrustProvider : ITrustProvider
    {
        public bool IsTrusted(ClaimsPrincipal principal)
        {
            return principal.IsInRole("admin");
        }
    }
}
