using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartHomeMock.SensorMock.Entities.Configuration
{
    public class SensorMockConfiguration
    {
        [JsonProperty("id")]
        public int ConfigurationId { get; set; }

        [JsonProperty("broker")]
        public Broker Broker { get; set; }

        [JsonProperty("modules")]
        public ICollection<Module> Modules { get; set; }
    }
}