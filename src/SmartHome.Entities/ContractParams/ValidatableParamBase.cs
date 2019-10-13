using System.ComponentModel.DataAnnotations;
using SmartHome.Core.Entities.Abstractions;

namespace SmartHome.Core.Entities.ContractParams
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
