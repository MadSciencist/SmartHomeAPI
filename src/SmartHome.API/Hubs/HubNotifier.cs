using Microsoft.AspNetCore.SignalR;
using SmartHome.Core.Domain.Notification;
using SmartHome.Core.Services;
using System;

namespace SmartHome.API.Hubs
{
    public class HubNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly NotificationQueue _queue;

        public HubNotifier(IHubContext<NotificationHub> hubContext, NotificationQueue queue, NotificationService notificationService)
        {
            _hubContext = hubContext;
            _queue = queue;
            notificationService.NotificationAdded += _notificationService_NotificationAdded;
        }

        private async void _notificationService_NotificationAdded(object sender, EventArgs e)
        {
            if (_queue.Count > 0)
            {
                NotificationDto message = _queue.Dequeue();
                await _hubContext.Clients.All.SendAsync("notification", message);
            }
        }
    }
}
