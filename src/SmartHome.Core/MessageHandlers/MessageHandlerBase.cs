using Autofac;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Services;
using System.Threading.Tasks;

namespace SmartHome.Core.MessageHandlers
{
    public abstract class MessageHandlerBase<T> where T: class, new()
    {
        protected ILifetimeScope Container { get; set; }

        private INodeDataService _nodeDataService;
        protected INodeDataService NodeDataService => _nodeDataService ?? (_nodeDataService = Container.Resolve<INodeDataService>());

        private NotificationService _notificationService;
        protected NotificationService NotificationService =>
            _notificationService ?? (_notificationService = Container.Resolve<NotificationService>());

        protected MessageHandlerBase(ILifetimeScope container)
        {
            Container = container;
        }

        public abstract Task Handle(Node node, T message);
    }
}
