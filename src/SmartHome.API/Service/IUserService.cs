using Matty.Framework;
using SmartHome.API.Dto;
using SmartHome.Core.Dto;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.API.Service
{
    public interface IUserService
    {
        ClaimsPrincipal Principal { get; set; }
        Task<ServiceResult<UserDto>> GetUserAsync(int userId);
        Task<ServiceResult<TokenDto>> LoginAsync(LoginDto login);
        Task<ServiceResult<TokenDto>> RegisterAsync(RegisterDto register);
        Task<ServiceResult<object>> UpdateAsync(RegisterDto update, int userId);
        Task<ServiceResult<object>> DeleteAsync(int userId);
    }
}