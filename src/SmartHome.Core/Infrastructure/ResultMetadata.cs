using Newtonsoft.Json;

namespace SmartHome.Core.Infrastructure
{
    public class ResultMetadata
    {
        [JsonProperty("errorDetails", NullValueHandling = NullValueHandling.Ignore)]
        public object ProblemDetails { get; set; }

        [JsonProperty("other", NullValueHandling = NullValueHandling.Ignore)]
        public object Other { get; set; }
    }
}