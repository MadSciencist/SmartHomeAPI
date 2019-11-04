using Matty.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Dto;
using SmartHome.API.Service;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUiConfigurationService _uiConfigService;

        public UsersController(IUserService userService, IUiConfigurationService uiConfigService, IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _uiConfigService = uiConfigService;
            _uiConfigService.Principal = contextAccessor.HttpContext.User;
            _userService.Principal = contextAccessor.HttpContext.User;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userService.GetUserAsync(id);

            return ControllerResponseHelper.GetDefaultResponse(result);
        }

        /// <summary>
        /// Login endpoint
        /// </summary>
        /// <param name="login"></param>
        /// <returns>JWT token</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(ServiceResult<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<TokenDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult<TokenDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var result = await _userService.LoginAsync(login);

            return ControllerResponseHelper.GetDefaultResponse(result);
        }

        /// <summary>
        /// Register endpoint
        /// </summary>
        /// <param name="register"></param>
        /// <returns>JWT token</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            var result = await _userService.RegisterAsync(register);

            return ControllerResponseHelper.GetDefaultResponse(result, StatusCodes.Status201Created);
        }

        /// <summary>
        ///  Removes user from system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<UserDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            return ControllerResponseHelper.GetDefaultResponse(result, StatusCodes.Status201Created);
        }

        #region UI Configuration

        /// <summary>
        /// Get configuration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("{userId}/config")]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<ServiceResult<UiConfigurationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ServiceResult<UiConfigurationDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<ServiceResult<UiConfigurationDto>>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetConfigurations(int userId, [FromQuery(Name = "type")]UiConfigurationType type)
        {
            // type was not specified, get all configurations
            if (type == (int)0)
            {
                var result = await _uiConfigService.GetUserConfigurations(userId);
                return ControllerResponseHelper.GetDefaultResponse(result);
            }
            else
            {
                var result = await _uiConfigService.GetUserConfigurationsByType(userId, type);
                return ControllerResponseHelper.GetDefaultResponse(result);
            }
        }


        /// <summary>
        /// Get configuration
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="userId"></param>
        [HttpGet("{userId}/config/{configId}")]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetConfiguration(int userId, int configId)
        {
            var result = await _uiConfigService.GetUserConfigurationById(userId, configId);

            return ControllerResponseHelper.GetDefaultResponse(result);
        }

        /// <summary>
        /// Create new configuration to hold UI settings
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="config">Parameter DTO</param>
        /// <returns>Created entity</returns>
        [HttpPost("{userId}/config")]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddConfiguration(int userId, UiConfigurationDto config)
        {
            var result = await _uiConfigService.AddConfiguration(userId, config);

            return ControllerResponseHelper.GetDefaultResponse(result, StatusCodes.Status201Created);
        }

        /// <summary>
        /// Create new configuration to hold UI settings
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="configId"></param>
        /// <param name="configDto"></param>
        /// <returns>Created entity</returns>
        [HttpPut("{userId}/config/{configId}")]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateConfiguration(int userId, int configId, UiConfigurationDto configDto)
        {
            var result = await _uiConfigService.UpdateUserConfiguration(userId, configId, configDto);

            return ControllerResponseHelper.GetDefaultResponse(result);
        }

        /// <summary>
        /// Create new configuration to hold UI settings
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="configId"></param>
        /// <returns>Created entity</returns>
        [HttpDelete("{userId}/config/{configId}")]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult<UiConfigurationDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteConfiguration(int userId, int configId)
        {
            var result = await _uiConfigService.DeleteUserConfiguration(userId, configId);

            return ControllerResponseHelper.GetDefaultResponse(result);
        }
        #endregion
    }
}
