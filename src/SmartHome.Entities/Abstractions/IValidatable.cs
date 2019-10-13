namespace SmartHome.Core.Entities.Abstractions
{
    public interface IValidatable<out T> where T : class, new()
    {
        T Validate();
    }
}
