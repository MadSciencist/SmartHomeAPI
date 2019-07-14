using FluentValidation;
using SmartHome.Core.Dto;
using SmartHome.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class ControlStrategyDtoValidator : AbstractValidator<ControlStrategyDto>
    {
        private readonly IDictionaryService _dictionaryService;
        IEnumerable<string> _validStrategies;

        public ControlStrategyDtoValidator(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
            Validate().Wait();
        }

        private async Task Validate()
        {
            var dict = await _dictionaryService.GetDictionaryByName("contracts");
            _validStrategies = dict.Data.Values.Select(x => x.InternalValue);

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ControlStrategyName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(250)
                .When(x => _validStrategies.Contains(x.ControlStrategyName.ToLowerInvariant()))
                .WithMessage(GetContextErrorMessage());
        }

        private string GetContextErrorMessage()
        {
            return "{PropertyName} must be one of: " + string.Join(", ", _validStrategies);
        }
    }
}
