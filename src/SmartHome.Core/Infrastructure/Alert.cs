using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartHome.Core.Infrastructure
{
    public class Alert
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType MessageType { get; set; }

        public Alert(string message, MessageType type)
        {
            Message = message;
            MessageType = type;
        }

        public Alert()
        {
        }
    }
}