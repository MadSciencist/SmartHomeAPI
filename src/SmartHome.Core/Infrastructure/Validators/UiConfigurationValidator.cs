using FluentValidation;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class UiConfigurationValidator : AbstractValidator<UiConfigurationDto>
    {
        public UiConfigurationValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty();

            RuleFor(x => x.Data)
                .NotEmpty();
        }
    }
}
