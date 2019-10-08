using MediatR;
using Newtonsoft.Json;
using SmartHome.Core.Entities.Models;
using System;

namespace SmartHome.Core.Dto
{
    public class NotificationDto : INotification
    {
        [JsonProperty("nodeId")]
        public int NodeId { get; }

        [JsonProperty("physicalProperty")]
        public PhysicalProperty PhysicalProperty { get; }

        [JsonProperty("value")]
        public string Value { get; }

        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; }

        public NotificationDto(int nodeId, PhysicalProperty physicalProperty, string value)
        {
            NodeId = nodeId;
            PhysicalProperty = physicalProperty;
            Value = value;
            TimeStamp = DateTime.UtcNow;
        }
    }
}
