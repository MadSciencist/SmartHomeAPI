using FluentValidation;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class NodeDtoValidator : AbstractValidator<NodeDto>
    {
        public NodeDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be empty");
        }
    }
}
