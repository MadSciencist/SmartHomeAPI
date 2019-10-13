using FluentValidation.Results;
using Matty.Framework;
using Matty.Framework.Enums;
using SmartHome.Core.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Core.Infrastructure.Validators
{
    public static class ValidatorExtensions
    {
        public static ICollection<Alert> GetValidationMessages(this ValidationResult result)
        {
            if (result is null)
            {
                throw new SmartHomeException(nameof(ValidationResult));
            }

            return result.Errors.Select(x => new Alert(x.ErrorMessage, MessageType.Error)).ToList();
        }
    }
}
