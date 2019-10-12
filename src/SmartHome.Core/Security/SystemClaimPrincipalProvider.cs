using Microsoft.IdentityModel.JsonWebTokens;
using SmartHome.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SmartHome.Core.Security
{
    public class SystemClaimPrincipalProvider
    {
        public static ClaimsPrincipal GetSystemClaimPrincipal()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
                new Claim(ClaimTypes.Name, "system"),
                new Claim(ClaimTypes.Email, "system@system.com"),
            };

            claims.AddRange(new[] {Roles.Admin, Roles.User}.Select(role => new Claim(ClaimTypes.Role, role)));

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}
