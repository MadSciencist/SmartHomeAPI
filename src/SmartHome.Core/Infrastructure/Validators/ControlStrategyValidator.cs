using FluentValidation;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class ControlStrategyValidator : AbstractValidator<ControlStrategyDto>
    {
        public ControlStrategyValidator()
        {
        }
    }
}
