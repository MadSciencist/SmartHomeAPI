using FluentValidation;
using SmartHome.Core.Dto;
using System;
using System.Linq;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class ControlStrategyDtoValidator : AbstractValidator<ControlStrategyDto>
    {
        private static readonly string[] VALID_CONTEXTS = { "rest", "mqtt" };

        public ControlStrategyDtoValidator()
        {
            RuleFor(x => x.ControlContext)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(50)
                .Must(x => VALID_CONTEXTS.Contains(x.ToLower()))
                .WithMessage(GetContextErrorMessage());

            RuleFor(x => x.ReceiveContext)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(50)
                .Must(x => VALID_CONTEXTS.Contains(x.ToLower()))
                .WithMessage(GetContextErrorMessage());

            RuleFor(x => x.ControlProvider)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.ReceiveProvider)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(50);
        }

        private string GetContextErrorMessage()
        {
            return "{PropertyName} must be one of: " + string.Join(", ", VALID_CONTEXTS);
        }
    }
}
