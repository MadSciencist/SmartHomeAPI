using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Configuration;
using SmartHomeMock.SensorMock.Entities.Data;
using SmartHomeMock.SensorMock.Entities.Enums;
using SmartHomeMock.SensorMock.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeMock.SensorMock.Domain
{
    public abstract class ModuleMockBase : IModuleMock
    {
        protected readonly IFactory<ESensorType, ISensorMock> SensorMockFactory;

        protected Module Module;
        protected Broker Broker;

        protected ISensorMock[] Sensors;

        protected ModuleMockBase(IFactory<ESensorType, ISensorMock> sensorMockFactory)
        {
            SensorMockFactory = sensorMockFactory;
        }

        public virtual void Initialize(Module module, Broker broker)
        {
            Module = module;
            Broker = broker;

            Sensors = Module.Sensors.Select(InitializeSensorMock).ToArray();
        }

        public virtual void Start()
        {
            foreach (var sensorMock in Sensors)
            {
                sensorMock.StateChanged += OnSensorStateChanged;
            }

            foreach (var sensorMock in Sensors)
            {
                StartSensorMock(sensorMock);
            }
        }

        protected ISensorMock InitializeSensorMock(Sensor sensor)
        {
            var sensorMock = SensorMockFactory.Get(sensor.Type);

            sensorMock.Initialize(sensor);

            return sensorMock;
        }

        protected void StartSensorMock(ISensorMock sensorMock)
        {
            sensorMock.Start();
        }

        protected abstract void OnSensorStateChanged(object sender, SensorData data);
    }
}
