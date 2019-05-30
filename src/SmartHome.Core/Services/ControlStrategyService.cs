using AutoMapper;
using FluentValidation;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.Validators;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class ControlStrategyService : ServiceBase<ControlStrategyDto, ControlStrategy>, IControlStrategyService
    {
        public ControlStrategyService(IGenericRepository<ControlStrategy> strategyRepository, IMapper mapper,
            IValidator<ControlStrategyDto> validator) : base(strategyRepository, mapper, validator)
        {
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
                return response;
            }
        }
    }
}
