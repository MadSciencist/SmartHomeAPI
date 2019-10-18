using Matty.Framework;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IUiConfigurationService : IServiceBase
    {
        /// <summary>
        /// Add new user UI configuration with specified type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="configDto"></param>
        /// <returns></returns>
        Task<ServiceResult<UiConfigurationDto>> AddConfiguration(int userId, UiConfigurationDto configDto);

        /// <summary>
        /// Get collection of all user configurations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<ICollection<UiConfigurationDto>>> GetUserConfigurations(int userId);

        /// <summary>
        /// Get specific user config by id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        Task<ServiceResult<UiConfigurationDto>> GetUserConfigurationById(int userId, int configId);

        /// <summary>
        /// Get specific user configs by type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ServiceResult<ICollection<UiConfigurationDto>>> GetUserConfigurationsByType(int userId, UiConfigurationType type);

        /// <summary>
        /// Update specific configuration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="configId"></param>
        /// <param name="configDto"></param>
        /// <returns></returns>
        Task<ServiceResult<UiConfigurationDto>> UpdateUserConfiguration(int userId, int configId, UiConfigurationDto configDto);

        /// <summary>
        /// Delete user config
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        Task<ServiceResult<object>> DeleteUserConfiguration(int userId, int configId);
    }
}
