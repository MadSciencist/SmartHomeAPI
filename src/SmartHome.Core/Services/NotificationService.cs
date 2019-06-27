using System;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Domain.Notification;

namespace SmartHome.Core.Services
{
    public class NotificationService
    {
        public event EventHandler NotificationAdded;

        private readonly NotificationQueue _queue;

        public NotificationService(NotificationQueue queue)
        {
            _queue = queue;
        }

        public void PushNotification(int nodeId, NotificationType type, string name, string magnitude)
        {
            _queue.Enqueue(new NotificationDto(nodeId, name, magnitude));
            NotificationAdded?.Invoke(this, EventArgs.Empty);
        }
    }
}
