using Matty.Framework.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Matty.Framework.Validation
{
    public abstract class ValidatableParamBase<T> : IValidatable<T> where T : class, new()
    {
        public virtual T Validate()
        {
            var ctx = new ValidationContext(this, null, null);
            Validator.ValidateObject(this, ctx, validateAllProperties: true);
            return this as T;
        }
    }
}
