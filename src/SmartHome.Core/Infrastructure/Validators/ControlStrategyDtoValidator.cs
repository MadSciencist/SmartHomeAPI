using FluentValidation;
using SmartHome.Core.Dto;
using System;
using System.Linq;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class ControlStrategyDtoValidator : AbstractValidator<ControlStrategyDto>
    {
        // TODO: use assembly scanning validation
        private static readonly string[] VALID_CONTEXTS = { "Rest", "Mqtt" };

        public ControlStrategyDtoValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ControlContext)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(50)
                .When(x => VALID_CONTEXTS.Contains(x.ControlContext.ToLowerInvariant()))
                .WithMessage(GetContextErrorMessage());

            RuleFor(x => x.ReceiveContext)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50)
                .When(x => VALID_CONTEXTS.Contains(x.ReceiveContext.ToLowerInvariant()))
                .WithMessage(GetContextErrorMessage());

            RuleFor(x => x.ControlProvider)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50);

            RuleFor(x => x.ReceiveProvider)
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50);
        }

        private static string GetContextErrorMessage()
        {
            return "{PropertyName} must be one of: " + string.Join(", ", VALID_CONTEXTS);
        }
    }
}
