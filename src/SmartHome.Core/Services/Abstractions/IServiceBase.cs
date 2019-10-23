using System.Security.Claims;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IServiceBase
    {
        ClaimsPrincipal Principal { get; set; }
    }
}