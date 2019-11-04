using SmartHome.Core.Entities.Enums;
using System.Security.Claims;

namespace SmartHome.API.Security
{
    public class AdminTrustProvider : ITrustProvider
    {
        public bool IsTrustedRequest(ClaimsPrincipal principal)
        {
            return principal.IsInRole(Roles.Admin);
        }
    }
}
