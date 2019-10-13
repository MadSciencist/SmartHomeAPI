using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using SmartHome.Core.Entities.Enums;

namespace SmartHome.Core.Entities.Utils
{
    public static class ClaimsPrincipalHelper
    {
        public static bool HasUserClaimedIdentifier(ClaimsPrincipal principal, string claimedId)
        {
            return GetClaimedIdentifier(principal).Equals(claimedId);
        }

        public static bool HasUserClaimedIdentifier(ClaimsPrincipal principal, int claimedId)
        {
            if (int.TryParse(GetClaimedIdentifier(principal), out int id))
            {
                return id == claimedId;
            }

            return false;
        }

        public static int GetClaimedIdentifierInt(ClaimsPrincipal principal)
        {
            return int.Parse(GetClaimedIdentifier(principal));
        }

        public static string GetClaimedIdentifier(ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        }

        public static bool IsUserAdmin(ClaimsPrincipal principal) => principal.IsInRole(Roles.Admin);

        public static IEnumerable<string> GetClaimedRoles(ClaimsPrincipal principal) =>
            principal.FindAll(ClaimTypes.Role).Select(x => x.Value);
    }
}
