using System;
using SmartHomeMock.SensorMock.Entities.Configuration;
using SmartHomeMock.SensorMock.Entities.Data;

namespace SmartHomeMock.SensorMock.Domain.Interfaces
{
    public interface ISensorMock
    {
        event EventHandler<SensorData> DataReceived;

        void Initialize(Sensor sensor);

        void Start();

        void Stop();

        void UpdateState();
    }
}