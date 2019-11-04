using Newtonsoft.Json;
using SmartHome.Core.Entities.Enums;

namespace SmartHome.Core.Dto
{
    public class UiConfigurationDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public UiConfigurationType Type { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
