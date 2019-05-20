namespace SmartHome.Core.MqttBroker.MessageHandling
{
    public class ReceivedMessage
    {
        public string ClientId { get; set; }
        public string Topic { get; set; }
        public string ContentType { get; set; }
        public string Payload { get; set; }
    }
}
