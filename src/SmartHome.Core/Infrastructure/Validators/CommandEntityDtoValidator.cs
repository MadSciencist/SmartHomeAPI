﻿using FluentValidation;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class CommandEntityDtoValidator : AbstractValidator<CommandEntityDto>
    {
        public CommandEntityDtoValidator()
        {
            RuleFor(x => x.Alias).NotNull().WithMessage("Alias cannot be empty");
            RuleFor(x => x.ExecutorClassName).NotNull().WithMessage("Class name cannot be empty");
        }
    }
}
