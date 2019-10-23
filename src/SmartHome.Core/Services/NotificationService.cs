using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NotificationService
    {
        // Todo: rewrite notification logic, user NotificationService mostly for authorization
        // Provide possibity of dispatching different types of messages
        private readonly IMediator _mediator;
        private readonly IDictionary<string, AppUser> _connectedUsers;
        private readonly UserManager<AppUser> _userManager;

        public NotificationService(UserManager<AppUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
            _connectedUsers = new Dictionary<string, AppUser>();
        }

        public void PushDataNotification(int nodeId, NodeDataDto dto)
        {
            var notification = new NotificationDto(nodeId, dto.PhysicalProperty, dto.Value);
            _mediator.Publish(notification);
        }

        public void PushDataNotification(int nodeId, IEnumerable<NodeDataDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var notification = new NotificationDto(nodeId, dto.PhysicalProperty, dto.Value);
                _mediator.Publish(notification);
            }
        }

        public async Task AddClientAsync(string connectionId, int userId)
        {
            var user = await _userManager
                .Users
                .Include(x => x.CreatedNodes)
                .Include(x => x.EligibleNodes)
                .SingleAsync(x => x.Id == userId);

            _connectedUsers.Add(connectionId, user);
        }

        public void RemoveClient(string connectionId)
        {
            _connectedUsers.Remove(connectionId);
        }
    }
}
