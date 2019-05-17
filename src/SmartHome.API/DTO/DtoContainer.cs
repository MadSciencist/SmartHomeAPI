using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace SmartHome.API.DTO
{
    public class ResponseDtoContainer<T> where T : class
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("messages")]
        public Collection<string> Messages { get; set; }

        [JsonProperty("errors")]
        public Collection<ErrorDetailsDto> Errors { get; set; }

        public ResponseDtoContainer()
        {
            Messages = new Collection<string>();
            Errors = new Collection<ErrorDetailsDto>();
        }
    }
}
