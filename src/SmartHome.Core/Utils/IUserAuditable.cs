using System.Security.Claims;

namespace SmartHome.Core.Utils
{
    public interface IUserAuditable
    {
        ClaimsPrincipal ClaimsOwner { get; set; }
    }
}
