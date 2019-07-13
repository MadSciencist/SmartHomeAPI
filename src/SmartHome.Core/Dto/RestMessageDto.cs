using Newtonsoft.Json.Linq;

namespace SmartHome.Core.Dto
{
    public class RestMessageDto
    {
        public string ClientId { get; set; }
        public JObject Payload { get; set; }
    }
}
