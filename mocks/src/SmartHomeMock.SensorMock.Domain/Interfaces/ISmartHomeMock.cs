using SmartHomeMock.SensorMock.Entities.Configuration;

namespace SmartHomeMock.SensorMock.Domain.Interfaces
{
    public interface ISmartHomeMock
    {
        void Run(int configurationId);
    }
}