using Newtonsoft.Json;
using SmartHome.Core.Domain.Models;

namespace SmartHome.Core.Domain.Notification
{
    public class NotificationDto
    {
        [JsonProperty("nodeId")]
        public int NodeId { get; }

        [JsonProperty("physicalProperty")]
        public PhysicalProperty PhysicalProperty { get; }

        [JsonProperty("value")]
        public string Value { get; }

        public NotificationDto(int nodeId, PhysicalProperty physicalProperty, string value)
        {
            NodeId = nodeId;
            PhysicalProperty = physicalProperty;
            Value = value;
        }
    }
}
