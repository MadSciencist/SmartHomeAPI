using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SmartHome.API.Tests
{
    public class Helpers
    {
        public static ControllerContext GetFakeHttpContextWithFakeAdminPrincipal()
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = GetFakeAdminPrincipal()

                }
            };
        }

        public static ClaimsPrincipal GetFakeAdminPrincipal()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));
        }
    }
}
