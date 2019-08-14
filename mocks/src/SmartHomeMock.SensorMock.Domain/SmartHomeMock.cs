using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet;
using SmartHomeMock.SensorMock.DataAccess.Interfaces;
using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Configuration;
using SmartHomeMock.SensorMock.Entities.Enums;
using SmartHomeMock.SensorMock.Infrastructure.Interfaces;

namespace SmartHomeMock.SensorMock.Domain
{
    public class SmartHomeMock : ISmartHomeMock
    {
        private readonly ISensorMockConfigurationRepository _sensorMockConfigurationRepository;
        private readonly IFactory<EModuleType, IModuleMock> _moduleMockFactory;

        private IModuleMock[] _moduleMocks;

        public SmartHomeMock(ISensorMockConfigurationRepository sensorMockConfigurationRepository, IFactory<EModuleType, IModuleMock> moduleMockFactory)
        {
            _sensorMockConfigurationRepository = sensorMockConfigurationRepository;
            _moduleMockFactory = moduleMockFactory;
        }

        public void Run(int configurationId)
        {
            var configuration = _sensorMockConfigurationRepository.GetConfigurationById(configurationId);

            _moduleMocks = configuration.Modules.Select(m => InitializeModuleMock(m, configuration.Broker)).ToArray();

            foreach(var moduleMock in _moduleMocks)
            {
                StartModuleMock(moduleMock);
            }
        }

        private IModuleMock InitializeModuleMock(Module module, Broker broker)
        {
            var moduleMock = _moduleMockFactory.Get(module.Type);

            moduleMock.Initialize(module, broker);

            return moduleMock;
        }

        private void StartModuleMock(IModuleMock moduleMock)
        {
            moduleMock.Start();
        }
    }
}