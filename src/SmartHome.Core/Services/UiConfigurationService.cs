﻿using Autofac;
using Matty.Framework;
using Matty.Framework.Enums;
using Matty.Framework.Utils;
using Microsoft.AspNetCore.Http;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class UiConfigurationService : CrudServiceBase<UiConfigurationDto, UiConfiguration>, IUiConfigurationService
    {
        public UiConfigurationService(ILifetimeScope container) : base(container)
        {
        }

        public async Task<ServiceResult<ICollection<UiConfigurationDto>>> GetUserConfigurations(int userId)
        {
            var response = new ServiceResult<ICollection<UiConfigurationDto>>(Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId)) || ClaimsPrincipalHelper.IsInRole(Principal, Roles.Admin))
            {
                response.Alerts.Add(new Alert($"You have no permissions to view configs of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var entities = await GenericRepository.GetManyFiltered(x => x.UserId == userId);

            if (entities is null || entities.Count() == 0)
            {
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            response.Data = Mapper.Map<ICollection<UiConfigurationDto>>(entities);
            return response;
        }

        public async Task<ServiceResult<UiConfigurationDto>> GetUserConfigurationById(int userId, int configId)
        {
            var response = new ServiceResult<UiConfigurationDto>(Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsInRole(Principal, Roles.Admin)))
            {
                response.Alerts.Add(new Alert($"You have no permission to view config {configId} of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var entity = await GenericRepository.GetFiltered(x => x.Id == configId);

            if (entity is null)
            {
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            response.Data = Mapper.Map<UiConfigurationDto>(entity);
            return response;
        }

        public async Task<ServiceResult<ICollection<UiConfigurationDto>>> GetUserConfigurationsByType(int userId, UiConfigurationType type)
        {
            var response = new ServiceResult<ICollection<UiConfigurationDto>>(Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsInRole(Principal, Roles.Admin)))
            {
                response.Alerts.Add(new Alert($"You have no permission to view config of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var entity = await GenericRepository.GetManyFiltered(x => x.UserId == userId && x.Type == type);

            if (entity is null)
            {
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            response.Data = Mapper.Map<List<UiConfigurationDto>>(entity);
            return response;
        }

        public async Task<ServiceResult<UiConfigurationDto>> AddConfiguration(int userId, UiConfigurationDto configDto)
        {
            var response = new ServiceResult<UiConfigurationDto>(Principal);

            if (configDto.Type == UiConfigurationType.Control || configDto.Type == UiConfigurationType.Dashboard)
            {
                var existing = await GenericRepository.GetManyFiltered(x =>
                    x.Type == UiConfigurationType.Control || x.Type == UiConfigurationType.Dashboard);

                switch (configDto.Type)
                {
                    case UiConfigurationType.Dashboard when existing.Any(x => x.Type == UiConfigurationType.Dashboard):
                        throw new SmartHomeException("Only one 'Dashboard' type config is allowed");
                    case UiConfigurationType.Control when existing.Any(x => x.Type == UiConfigurationType.Control):
                        throw new SmartHomeException("Only one 'Control' type config is allowed");
                }
            }

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
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsInRole(Principal, Roles.Admin)))
            {
                response.Alerts.Add(new Alert($"You have no permission to view config {configId} of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var entity = await GenericRepository.GetFiltered(x => x.Id == configId);

            entity.Data = configDto.Data;
            entity.Type = configDto.Type;

            var updated = await GenericRepository.UpdateAsync(entity);

            response.Data = Mapper.Map<UiConfigurationDto>(updated);
            return response;
        }

        public async Task<ServiceResult<object>> DeleteUserConfiguration(int userId, int configId)
        {
            var response = new ServiceResult<object>(Principal);

            // Simple authorization - only user itself or admin can access it
            if (!(ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId) || ClaimsPrincipalHelper.IsInRole(Principal, Roles.Admin)))
            {
                response.Alerts.Add(new Alert($"You have no permission to view config {configId} of user {userId}.", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status403Forbidden;
                return response;
            }

            var config = await GenericRepository.GetByIdAsync(configId);
            if (config is null)
            {
                response.Alerts.Add(new Alert("Config with given ID does not exist", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
            }
            else
            {
                await GenericRepository.DeleteAsync(config);
                response.Alerts.Add(new Alert("Successfully deleted", MessageType.Success));
            }

            return response;
        }
    }
}
