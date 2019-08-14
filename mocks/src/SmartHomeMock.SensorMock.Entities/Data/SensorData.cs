using SmartHomeMock.SensorMock.Entities.Configuration;

namespace SmartHomeMock.SensorMock.Entities.Data
{
    public class SensorData
    {
        public Sensor Sensor { get; set; }
        public string[] Values { get; set; }
        public string Payload { get; set; }
    }
}