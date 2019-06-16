using Autofac;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class ControlStrategyService : ServiceBase<ControlStrategyDto, object>, IControlStrategyService
    {
        private readonly IStrategyRepository _strategyRepository;

        public ControlStrategyService(ILifetimeScope container, IStrategyRepository strategyRepository) : base(container)
        {
            _strategyRepository = strategyRepository;
        }

        public async Task<ServiceResult<IEnumerable<ControlStrategyDto>>> GetAll()
        {
            var response = new ServiceResult<IEnumerable<ControlStrategyDto>>(Principal);

            try
            {
                var strategies = await _strategyRepository.GetAll();
                var mapped = Mapper.Map<IEnumerable<ControlStrategyDto>>(strategies).ToList();

                foreach (var map in mapped)
                {
                    map.Commands = strategies.First(x => x.Id == map.Id).ControlStrategyLinkages
                        .Where(x => x.ControlStrategyLinkageTypeId == (int)ELinkageType.Command)
                        .Select(x => new ControlStrategyLinkageDto(x.DisplayValue, x.InternalValue))
                        .ToList();

                    map.Sensors = strategies.First(x => x.Id == map.Id).ControlStrategyLinkages
                        .Where(x => x.ControlStrategyLinkageTypeId == (int)ELinkageType.Sensor)
                        .Select(x => new ControlStrategyLinkageDto(x.DisplayValue, x.InternalValue))
                        .ToList();
                }

                response.Data = mapped;

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        public async Task<ServiceResult<ControlStrategyDto>> CreateStrategy(ControlStrategyDto input)
        {
            var response = new ServiceResult<ControlStrategyDto>(Principal);
            var validationResult = Validator.Validate(input);

            if (!validationResult.IsValid)
            {
                response.Alerts = validationResult.GetValidationMessages();
                return response;
            }

            var userId = GetCurrentUserId();
            var strategyToCreate = Mapper.Map<ControlStrategy>(input);

            strategyToCreate.CreatedById = userId;
            strategyToCreate.Created = DateTime.UtcNow;
            strategyToCreate.IsActive = true;

            using (var transaction = _strategyRepository.Context.Database.BeginTransaction())
            {
                try
                {
                    var created = await _strategyRepository.CreateAsync(strategyToCreate);

                    var commandLinkages = new List<ControlStrategyLinkage>();
                    foreach (var command in input.Commands)
                    {
                        commandLinkages.Add(new ControlStrategyLinkage
                        {
                            ControlStrategyLinkageTypeId = (int)ELinkageType.Command,
                            ControlStrategyId = created.Id,
                            InternalValue = command.InternalValue,
                            DisplayValue = command.DisplayValue
                        });
                    }

                    foreach (var sensor in input.Sensors)
                    {
                        commandLinkages.Add(new ControlStrategyLinkage
                        {
                            ControlStrategyLinkageTypeId = (int)ELinkageType.Sensor,
                            ControlStrategyId = created.Id,
                            InternalValue = sensor.InternalValue,
                            DisplayValue = sensor.DisplayValue
                        });
                    }

                    _strategyRepository.Context.ControlStrategyLinkages.AddRange(commandLinkages);
                    await _strategyRepository.Context.SaveChangesAsync();

                    response.Data = Mapper.Map<ControlStrategyDto>(created);
                    response.Alerts.Add(new Alert("Successfully created", MessageType.Success));

                    transaction.Commit();
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                    throw;
                }
            }
        }
    }
}
