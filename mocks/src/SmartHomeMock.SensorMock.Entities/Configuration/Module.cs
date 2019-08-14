using System.Collections.Generic;
using Newtonsoft.Json;
using SmartHomeMock.SensorMock.Entities.Enums;

namespace SmartHomeMock.SensorMock.Entities.Configuration
{
    public class Module
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("type")]
        public EModuleType Type { get; set; }

        [JsonProperty("sensors")]
        public ICollection<Sensor> Sensors { get; set; }

        [JsonProperty("listeners")]
        public ICollection<Listener> Listeners { get; set; }
    }
}