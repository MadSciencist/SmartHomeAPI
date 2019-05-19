using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Core.Infrastructure.Validators
{
    public static class ValidatorExtensions 
    {
        public static ICollection<Alert> GetValidationMessages(this ValidationResult result)
        {
            if (result == null)
            {
                throw  new SmartHomeException(nameof(ValidationResult));
            }

            return result.Errors.Select(x => new Alert(x.ErrorMessage, MessageType.Error)).ToList();
        }
    }
}
