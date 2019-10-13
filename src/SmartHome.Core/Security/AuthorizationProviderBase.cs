using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.Utils;
using System.Security.Claims;
using System.Security.Principal;

namespace SmartHome.Core.Security
{
    public abstract class AuthorizationProviderBase
    {
        protected static bool IsAdmin(IPrincipal principal)
        {
            return principal.IsInRole(Roles.Admin);
        }

        protected static bool IsSystemUser(ClaimsPrincipal principal)
        {
            return ClaimsPrincipalHelper.GetClaimedIdentifier(principal) == "1";
        }
    }
}
