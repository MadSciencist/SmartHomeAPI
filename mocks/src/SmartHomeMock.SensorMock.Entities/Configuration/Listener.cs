using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHomeMock.SensorMock.Entities.Configuration
{
    public class Listener
    {
        [JsonProperty("sensorId")]
        public string SensorId { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }
}
