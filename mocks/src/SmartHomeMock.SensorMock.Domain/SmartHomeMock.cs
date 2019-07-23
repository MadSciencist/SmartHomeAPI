using MQTTnet;
using SmartHomeMock.SensorMock.DataAccess.Interfaces;
using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Configuration;

namespace SmartHomeMock.SensorMock.Domain
{
    public class SmartHomeMock : ISmartHomeMock
    {
        private readonly ISensorMockConfigurationRepository _sensorMockConfigurationRepository;

        public SmartHomeMock(ISensorMockConfigurationRepository sensorMockConfigurationRepository)
        {
            _sensorMockConfigurationRepository = sensorMockConfigurationRepository;
        }

        public void Run(int configurationId)
        {
            var configuration = _sensorMockConfigurationRepository.GetConfigurationById(configurationId);


        }
    }
}