using Newtonsoft.Json;
using SmartHomeMock.SensorMock.Entities.Enums;

namespace SmartHomeMock.SensorMock.Entities.Configuration
{
    public class Sensor
    {
        [JsonProperty("type")]
        public ESensorType Type { get; set; }

        [JsonProperty("unit")]
        public EUnit Unit { get; set; }

        [JsonProperty("interval")]
        public int Interval { get; set; }
    }
}