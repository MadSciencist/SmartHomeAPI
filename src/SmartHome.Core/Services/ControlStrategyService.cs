using Autofac;
using Matty.Framework;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Repositories;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matty.Framework.Extensions;
using SmartHome.Core.Infrastructure.Exceptions;

namespace SmartHome.Core.Services
{
    public class ControlStrategyService : ServiceBase, IControlStrategyService
    {
        private readonly IControlStrategyRepository _strategyRepository;

        public ControlStrategyService(ILifetimeScope container, IControlStrategyRepository strategyRepository) : base(container)
        {
            _strategyRepository = strategyRepository;
        }

        public async Task<ServiceResult<IEnumerable<ControlStrategyDto>>> GetAll()
        {
            var strategies = await _strategyRepository.GetAllAsync();

            return new ServiceResult<IEnumerable<ControlStrategyDto>>(Principal)
            {
                Data = Mapper.Map<IEnumerable<ControlStrategyDto>>(strategies)
            };
        }

        public async Task<ServiceResult<ControlStrategyDto>> Create(ControlStrategyDto dto)
        {
            var result = new ServiceResult<ControlStrategyDto>(Principal);
            var strategy = Mapper.Map<ControlStrategy>(dto);
            strategy.ContractAssembly = AssemblyScanner.GetAssemblyModuleNameByProductInfo(dto.Connector);
            strategy.IsActive = true;

            try
            {
                var physicalPropertyIds = dto.PhysicalProperties.Select(x => x.Id);
                var created = await _strategyRepository.CreateWithPropertyLinksAsync(strategy, physicalPropertyIds);
                var cratedWithRelatedData = await _strategyRepository.GetByIdAsync(created.Id);
                result.Data = Mapper.Map<ControlStrategyDto>(cratedWithRelatedData);
                result.AddSuccessMessage("Successfully created new control strategy");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occured while creating new strategy.", ex);
            }

            return result;
        }

        public async Task<ServiceResult<ControlStrategyDto>> Update(int id, ControlStrategyDto dto)
        {
            var result = new ServiceResult<ControlStrategyDto>(Principal);

            var existing = await _strategyRepository.GetByIdAsync(id);
            if (existing is null) throw new SmartHomeEntityNotFoundException($"ControlStrategy with id: {id} not found.");

            // This needs to be refactored - need a way, to update only certain fields without breaking OPEN/CLOSED principle
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.IsActive = dto.IsActive;

            // If connector changed, we also need to update contract assembly
            if (!string.IsNullOrEmpty(dto.Connector))
            {
                existing.Connector = dto.Connector;
                existing.ContractAssembly = AssemblyScanner.GetAssemblyModuleNameByProductInfo(dto.Connector);
            }

            var physicalPropertiesToUpdate = existing.PhysicalProperties.ToList();
            var dtoProperties = dto.PhysicalProperties.ToList();

            foreach (var physicalProp in dtoProperties)
            {
                // Check if there is a new link request, if so add it
                if (!physicalPropertiesToUpdate.Any(x => x.PhysicalPropertyId == physicalProp.Id))
                {
                    physicalPropertiesToUpdate.Add(new PhysicalPropertyControlStrategyLink
                    {
                        ControlStrategyId = existing.Id,
                        PhysicalPropertyId = physicalProp.Id
                    });
                }
            }

            foreach (var existingProp in existing.PhysicalProperties)
            {
                if (!dto.PhysicalProperties.Any(x => x.Id == existingProp.PhysicalPropertyId))
                {
                    physicalPropertiesToUpdate.Remove(existingProp);
                }
            }

            try
            {
                var updated = await _strategyRepository.UpdateWithLinksAsync(existing, physicalPropertiesToUpdate);
                var withRelatedData = await _strategyRepository.GetByIdAsync(updated.Id);
                result.Data = Mapper.Map<ControlStrategyDto>(withRelatedData);
                result.AddSuccessMessage("Successfully updated control strategy");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occured while creating new strategy.", ex);
            }

            return result;
        }

        public async Task<ServiceResult<object>> Delete(int id)
        {
            var existing = await _strategyRepository.GetByIdAsync(id);
            if (existing is null) throw new SmartHomeEntityNotFoundException($"ControlStrategy with id: {id} not found.");

            await _strategyRepository.DeleteAsync(existing);

            var result = new ServiceResult<object>(Principal);
            result.AddSuccessMessage("Successfully removed strategy");
            return result;
        }
    }
}
