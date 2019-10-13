namespace Matty.Framework.Abstractions
{
    public interface IValidatable<T> where T : class, new()
    {
        T Validate();
    }
}
