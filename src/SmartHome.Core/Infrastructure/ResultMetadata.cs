using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SmartHome.Core.Infrastructure
{
    public class ResultMetadata
    {
        [JsonProperty("errorDetails")]
        public ProblemDetails ProblemDetails { get; set; }

        [JsonProperty("other")]
        public object Other { get; set; }
    }
}