using Newtonsoft.Json;
using SmartHomeMock.SensorMock.Entities.Enums;

namespace SmartHomeMock.SensorMock.Entities.Configuration
{
    public class Sensor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public ESensorType Type { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("unit")]
        public EUnit Unit { get; set; }

        [JsonProperty("interval")]
        public int Interval { get; set; }
    }
}