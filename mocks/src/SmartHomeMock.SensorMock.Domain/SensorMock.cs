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

        public event EventHandler<SensorData> StateChanged;

        public void Initialize(Sensor sensor)
        {
            _sensor = sensor;
        }

        public void Start()
        {
            if (_sensor.Interval == 0)
            {
                return;
            }

            if (StateChanged is null)
            {
                throw new Exception("StateChanged event is null");
            }

            // TODO - Add cancellation token
            Task.Run(async () =>
            {
                while (true)
                {
                    var newData = GenerateNewData();

                    StateChanged?.Invoke(this, newData);

                    await Task.Delay(_sensor.Interval);
                }
            });
        }

        public void Stop()
        {
            // TODO - Implement Stop method (using cancellation token) 
            throw new NotImplementedException();
        }

        public void UpdateState(SensorData data)
        {
            if (StateChanged is null)
            {
                throw new Exception("StateChanged event is null");
            }

            StateChanged.Invoke(this, data);
        }

        private SensorData GenerateNewData()
        {
            return new SensorData();
        }
    }
}