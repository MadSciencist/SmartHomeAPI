using System;
using Newtonsoft.Json;

namespace SmartHome.API.Dto
{
    public class ErrorDetailsDto
    {
        public Guid CorrelationId { get; set; }
        public DateTime Time { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonProperty("requestDetails")]
        public ErrorDetailsLocationDto Location { get; set; }
    }

    public class ErrorDetailsLocationDto
    {
        public string Method { get; set; }
        public string Path { get; set; }
    }
}
