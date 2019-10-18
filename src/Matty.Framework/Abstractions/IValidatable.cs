namespace Matty.Framework.Abstractions
{
    public interface IThrowableValidator<out T> where T : class, new()
    {
        /// <summary>
        /// Perform validation. Should throw ValidationException when fail.
        /// </summary>
        /// <returns>Entity that was validated.</returns>
        T Validate();
    }
}
