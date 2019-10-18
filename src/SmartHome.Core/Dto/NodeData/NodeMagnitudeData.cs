using Newtonsoft.Json;
using System.Collections.Generic;

namespace SmartHome.Core.Dto.NodeData
{
    public class NodeMagnitudeData
    {
        public string Magnitude { get; set; }

        public string Unit { get; set; }

        [JsonProperty("records")]
        public ICollection<NodeMagnitudeRecord> Data { get; set; }
    }
}
