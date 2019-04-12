using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SmartHome.API.Security.Utils
{
    public static class ClaimsPrincipalHelper
    {
        public static bool HasUserClaimedIdentifier(ClaimsPrincipal principal, string claimedId)
        {
            return GetClaimedIdentifier(principal).Equals(claimedId);
        }

        public static string GetClaimedIdentifier(ClaimsPrincipal principal)
        {
            //return principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        }

        public static bool IsUserAdmin(ClaimsPrincipal principal) => GetClaimedRoles(principal).Contains("admin");

        public static IEnumerable<string> GetClaimedRoles(ClaimsPrincipal principal) =>
            principal.FindAll(ClaimTypes.Role).Select(x => x.Value);
    }
}
