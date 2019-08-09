using Newtonsoft.Json;

namespace SmartHomeMock.SensorMock.Entities.Configuration
{
    public class Broker
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
    }
}