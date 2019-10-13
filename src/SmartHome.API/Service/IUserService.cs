using SmartHome.API.Dto;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Matty.Framework;

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