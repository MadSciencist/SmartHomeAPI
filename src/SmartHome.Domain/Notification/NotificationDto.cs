namespace SmartHome.Core.Domain.Notification
{
    public class NotificationDto
    {
        public int NodeId { get; }
        public string Name { get; }
        public string Magnitude { get; }

        public NotificationDto(int nodeId, string name, string magnitude)
        {
            NodeId = nodeId;
            Name = name;
            Magnitude = magnitude;
        }
    }
}
