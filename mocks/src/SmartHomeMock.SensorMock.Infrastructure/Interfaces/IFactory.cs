namespace SmartHomeMock.SensorMock.Infrastructure.Interfaces
{
    public interface IFactory<in TKey, out TService>
    {
        TService Get(TKey key);
    }
}