namespace SmartHome.Core.Dto
{
    public class MqttMessageDto
    {
        public string ClientId { get; set; }
        public string Topic { get; set; }
        public string ContentType { get; set; }
        public string Payload { get; set; }
    }
}
