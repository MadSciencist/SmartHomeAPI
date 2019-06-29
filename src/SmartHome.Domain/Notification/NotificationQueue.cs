using System.Collections.Concurrent;

namespace SmartHome.Core.Domain.Notification
{
    public class NotificationQueue : ConcurrentQueue<NotificationDto>
    {
    }
}
