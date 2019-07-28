using MediatR;
using Microsoft.AspNetCore.SignalR;
using SmartHome.Core.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.API.Hubs
{
    public class NotificationHandler : INotificationHandler<NotificationDto>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHandler(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(NotificationDto dto, CancellationToken ctoken)
        {
            await _hubContext.Clients.All.SendAsync("notification", dto, ctoken);
        }
    }
}
