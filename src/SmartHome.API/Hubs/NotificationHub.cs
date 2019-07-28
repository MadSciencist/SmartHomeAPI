using Microsoft.AspNetCore.SignalR;
using SmartHome.API.Security.Token;
using SmartHome.Core.Services;
using System;
using System.Threading.Tasks;

namespace SmartHome.API.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly HubTokenDecoder _tokenDecoder;
        private readonly NotificationService _notificationService;

        public NotificationHub(HubTokenDecoder tokenDecoder, NotificationService notificationService)
        {
            _tokenDecoder = tokenDecoder;
            _notificationService = notificationService;
        }

        public override async Task OnConnectedAsync()
        {
            var (isAuth, id) = _tokenDecoder.GetClaimValues(Context);

            if (isAuth && id.HasValue) 
            {
                await _notificationService.AddClientAsync(Context.ConnectionId, id.Value);
                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _notificationService.RemoveClient(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
