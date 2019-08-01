using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class UiConfigurationService : ServiceBase<UiConfigurationDto, UiConfiguration>, IUiConfigurationService
    {
        public UiConfigurationService(ILifetimeScope container) : base(container)
        {
        }

        public async Task<ServiceResult<ICollection<UiConfigurationDto>>> GetUserConfigurations(int userId)
        {
            var response = new ServiceResult<ICollection<UiConfigurationDto>> (Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsUserAdmin(Principal)))
            {
                response.Alerts.Add(new Alert($"You have no permissions to view configs of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }
            var entities = await GenericRepository.AsQueryableNoTrack()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if(entities is null || entities.Count == 0)
            {
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            response.Data = Mapper.Map<ICollection<UiConfigurationDto>>(entities);
            return response;
        }

        public async Task<ServiceResult<UiConfigurationDto>> GetUserConfiguration(int userId, int configId)
        {
            var response = new ServiceResult<UiConfigurationDto>(Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsUserAdmin(Principal)))
            {
                response.Alerts.Add(new Alert($"You have no permission to view config {configId} of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var entity = await GenericRepository.AsQueryableNoTrack()
                .FirstOrDefaultAsync(x => x.Id == configId);

            if (entity is null)
            {
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            response.Data = Mapper.Map<UiConfigurationDto>(entity);
            return response;
        }

        public async Task<ServiceResult<UiConfigurationDto>> GetUserConfiguration(int userId, UiConfigurationType type)
        {
            var response = new ServiceResult<UiConfigurationDto>(Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsUserAdmin(Principal)))
            {
                response.Alerts.Add(new Alert($"You have no permission to view config of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var entity = await GenericRepository.AsQueryableNoTrack()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Type == type);

            if (entity is null)
            {
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            response.Data = Mapper.Map<UiConfigurationDto>(entity);
            return response;
        }

        public async Task<ServiceResult<UiConfigurationDto>> AddConfiguration(int userId, UiConfigurationDto configDto)
        {
            var response = new ServiceResult<UiConfigurationDto>(Principal);

            var entity = Mapper.Map<UiConfiguration>(configDto);
            entity.UserId = userId;

            var newEntity = await GenericRepository.CreateAsync(entity);

            response.Data = Mapper.Map<UiConfigurationDto>(newEntity);
            return response;
        }

        public async Task<ServiceResult<UiConfigurationDto>> UpdateUserConfiguration(int userId, int configId, UiConfigurationDto configDto)
        {
            var response = new ServiceResult<UiConfigurationDto>(Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsUserAdmin(Principal)))
            {
                response.Alerts.Add(new Alert($"You have no permission to view config {configId} of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var entity = await GenericRepository.AsQueryableNoTrack()
                .FirstOrDefaultAsync(x => x.Id == configId);

            entity.Data = configDto.Data;
            entity.Type = configDto.Type;

            var updated = await GenericRepository.UpdateAsync(entity);

            response.Data = Mapper.Map<UiConfigurationDto>(updated);
            return response;
        }
    }
}
