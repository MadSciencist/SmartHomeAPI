using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Domain.Models;
using SmartHome.Core.Domain.Notification;
using SmartHome.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class NotificationService
    {
        public event EventHandler NotificationAdded;

        private readonly IDictionary<string, AppUser> _connectedUsers;
        private readonly NotificationQueue _queue;
        private readonly UserManager<AppUser> _userManager;

        public NotificationService(NotificationQueue queue, UserManager<AppUser> userManager)
        {
            _queue = queue;
            _userManager = userManager;
            _connectedUsers = new Dictionary<string, AppUser>();
        }

        public void PushNodeDataNotification(int nodeId, PhysicalProperty physicalValue, string value)
        {
            _queue.Enqueue(new NotificationDto(nodeId, physicalValue, value));
            NotificationAdded?.Invoke(this, EventArgs.Empty);
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
