using FluentValidation;
using SmartHome.Core.Dto;
using System;
using System.Linq;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class ControlStrategyDtoValidator : AbstractValidator<ControlStrategyDto>
    {
        // TODO: use assembly scanning validation - dictionary service
        private static readonly string[] VALID_CONTEXTS = { "SmartHome.Contracts.EspurnaMqtt", "SmartHome.Contracts.EspurnaRest" };

        public ControlStrategyDtoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ContractAssembly)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(50)
                .When(x => VALID_CONTEXTS.Contains(x.ContractAssembly.ToLowerInvariant()))
                .WithMessage(GetContextErrorMessage());
        }

        private static string GetContextErrorMessage()
        {
            return "{PropertyName} must be one of: " + string.Join(", ", VALID_CONTEXTS);
        }
    }
}
