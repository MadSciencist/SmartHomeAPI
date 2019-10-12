namespace SmartHome.Core.Entities.ContractParams
{
    interface IValidatable<T> where T : class, new()
    {
        T Validate();
    }
}
