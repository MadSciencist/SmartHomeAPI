using SmartHomeMock.SensorMock.Entities;
using SmartHomeMock.SensorMock.Entities.Configuration;

namespace SmartHomeMock.SensorMock.DataAccess.Interfaces
{
    public interface ISensorMockConfigurationRepository
    {
        SensorMockConfiguration GetConfigurationById(int id);
    }
}