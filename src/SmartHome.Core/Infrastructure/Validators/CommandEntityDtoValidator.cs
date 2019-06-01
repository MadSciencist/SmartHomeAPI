using FluentValidation;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class CommandEntityDtoValidator : AbstractValidator<CommandEntityDto>
    {
        public CommandEntityDtoValidator()
        {
            RuleFor(x => x.Alias)
                .NotNull()
                .MaximumLength(50);

            RuleFor(x => x.ExecutorClassName)
                .NotNull()
                .MaximumLength(100);
        }
    }
}
