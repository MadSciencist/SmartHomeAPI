using FluentValidation;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class ControlStrategyValidator : AbstractValidator<ControlStrategyDto>
    {
        public ControlStrategyValidator()
        {
            RuleFor(x => x.ControlContext).NotNull();
            RuleFor(x => x.ControlProvider).NotNull();
            RuleFor(x => x.ReceiveContext).NotNull();
            RuleFor(x => x.ReceiveProvider).NotNull();
        }
    }
}
