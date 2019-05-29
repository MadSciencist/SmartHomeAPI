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
    public class ControlStrategyService : ServiceBase, IControlStrategyService
    {
        private readonly IGenericRepository<ControlStrategy> _strategyRepository;
        private readonly IValidator<ControlStrategyDto> _validator;

        public ControlStrategyService(IGenericRepository<ControlStrategy> strategyRepository, IMapper mapper, IValidator<ControlStrategyDto> validator) : base(mapper)
        {
            _strategyRepository = strategyRepository;
            _validator = validator;
        }

        public async Task<ServiceResult<ControlStrategyDto>> CreateStrategy(ControlStrategyDto input)
        {
            var response = new ServiceResult<ControlStrategyDto>();
            var validationResult = _validator.Validate(input);

            if (!validationResult.IsValid)
            {
                response.Alerts = validationResult.GetValidationMessages();
                return response;
            }

            var userId = GetCurrentUserId(Principal);
            var strategyToCreate = Mapper.Map<ControlStrategy>(input);

            strategyToCreate.CreatedById = userId;
            strategyToCreate.Created = DateTime.UtcNow;

            try
            {
                var created = await _strategyRepository.CreateAsync(strategyToCreate);
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
