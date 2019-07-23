using System;
using System.Threading.Tasks;
using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Configuration;
using SmartHomeMock.SensorMock.Entities.Data;

namespace SmartHomeMock.SensorMock.Domain
{
    public class SensorMock : ISensorMock
    {
        private Sensor _sensor;

        public event EventHandler<SensorData> DataReceived;

        public void Initialize(Sensor sensor)
        {
            _sensor = sensor;
        }

        public void Start()
        {
            while (true)
            {
                Task.Delay(100);
                // Random data

                DataReceived.Invoke(null, null);
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void UpdateState()
        {
            DataReceived.Invoke(null, null);
        }
    }
}