using AutoMapper;
using FluentValidation;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.Core.Services
{
    public class ControlStrategyService : ServiceBase<ControlStrategyDto, object>, IControlStrategyService
    {
        private readonly IStrategyRepository _strategyRepository;
        private readonly IGenericRepository<Command> _commandRepository;
        private readonly IGenericRepository<ControlStrategyCommandLink> _strategyCommandLinkRepository;

        public ControlStrategyService(
            IStrategyRepository strategyRepository,
            IGenericRepository<Command> commandRepository,
            IGenericRepository<ControlStrategyCommandLink> strategyCommandLinkRepository,
            IMapper mapper,
            IValidator<ControlStrategyDto> validator) : base(mapper, validator)
        {
            _strategyRepository = strategyRepository;
            _commandRepository = commandRepository;
            _strategyCommandLinkRepository = strategyCommandLinkRepository;
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

            try
            {
                var created = await GenericRepository.CreateAsync(strategyToCreate);
                response.Data = Mapper.Map<ControlStrategyDto>(created);
                response.Alerts.Add(new Alert("Successfully created", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        // TODO move to command service, create command controller
        public async Task<ServiceResult<CommandEntityDto>> CreateCommand(string alias, string executorClass)
        {
            var response = new ServiceResult<CommandEntityDto>(Principal);

            var command = new Command
            {
                Alias = alias,
                ExecutorClassName = executorClass
            };

            //Todo command validator here

            try
            {
                var created = await _commandRepository.CreateAsync(command);
                response.Data = response.Data = Mapper.Map<CommandEntityDto>(created);
                response.Alerts.Add(new Alert("Successfully created", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }           
        }

        public async Task<ServiceResult<ControlStrategyDto>> AttachAvailableCommand(int strategyId, int commandId)
        {
            var response = new ServiceResult<ControlStrategyDto>(Principal);

            var strategy = await _strategyRepository.GetByIdAsync(strategyId);
            if(strategy == null) throw new SmartHomeEntityNotFoundException($"Cannot find strategy with given Id: {strategyId}");

            var command = await _commandRepository.GetByIdAsync(commandId);
            if (command == null) throw new SmartHomeEntityNotFoundException($"Cannot find command with given Id: {commandId}");

            var linkEntity = await _strategyCommandLinkRepository
                .AsQueryableNoTrack()
                .FirstOrDefaultAsync(x => x.ControlStrategyId == strategyId && x.CommandId == commandId);
            if(linkEntity != null) throw new SmartHomeException($"Command {command.Alias} is already attached to strategy {strategy.Id}");

            try
            {
                await _strategyCommandLinkRepository.CreateAsync(new ControlStrategyCommandLink
                {
                    CommandId = commandId,
                    ControlStrategyId = strategyId
                });

                response.Data = Mapper.Map<ControlStrategyDto>(strategy);
                response.Data.AllowedCommands = Mapper.Map<ICollection<CommandEntityDto>>(strategy.AllowedCommands.Select(x => x.Command)); // TODO move to AutoMapper
                response.Alerts.Add(new Alert("Successfully created", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }
    }
}
