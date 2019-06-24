using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SmartHome.API.Security.Token;
using SmartHome.Core.Domain.Notification;
using SmartHome.Core.Services;

namespace SmartHome.API.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly HubTokenDecoder _tokenDecoder;

        public NotificationHub(HubTokenDecoder tokenDecoder)
        {
            _tokenDecoder = tokenDecoder;
        }

        public override Task OnConnectedAsync()
        {
            var (isAuth, id) = _tokenDecoder.GetClaimValues(Context);
            var connectionid = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
