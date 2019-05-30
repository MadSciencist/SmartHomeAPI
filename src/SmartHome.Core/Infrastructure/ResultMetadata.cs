using Newtonsoft.Json;

namespace SmartHome.Core.Infrastructure
{
    public class ResultMetadata
    {
        [JsonProperty("errorDetails")]
        public object ProblemDetails { get; set; }

        [JsonProperty("other")]
        public object Other { get; set; }
    }
}