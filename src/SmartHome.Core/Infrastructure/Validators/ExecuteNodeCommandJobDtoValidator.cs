using FluentValidation;
using Quartz;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure.Validators
{
    public class ExecuteNodeCommandJobDtoValidator : AbstractValidator<ScheduleNodeCommandJobDto>
    {
        public ExecuteNodeCommandJobDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.NodeId)
                .NotEmpty()
                .GreaterThan(1);

            RuleFor(x => x.JobTypeId)
                .NotEmpty()
                .GreaterThan(1);

            RuleFor(x => x.CronExpression)
                .NotEmpty()
                .Must(ValidateAgainstRegex)
                .WithMessage("Given expression is invalid. Please refer to Quartz.NET documentation.");

            RuleFor(x => x.Command)
                .NotEmpty(); // TODO validate against existing commands
        }

        private static bool ValidateAgainstRegex(string expression)
        {
            return CronExpression.IsValidExpression(expression);
        }
    }
}