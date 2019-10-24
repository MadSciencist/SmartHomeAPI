using Autofac;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.Services;
using SmartHome.Core.Services.Abstractions;
using System.Threading.Tasks;

namespace SmartHome.Core.Control
{
    public abstract class ControlCommandBase
    {
        protected ILifetimeScope Container { get; }
        protected Node Node { get; }

        protected ControlCommandBase(ILifetimeScope container, Node node)
        {
            Container = container;
            Node = node;
        }

        private NotificationService _notificationService;
        protected NotificationService NotificationService =>
            _notificationService ?? (_notificationService = Container.Resolve<NotificationService>());

        private INodeDataService _nodeDataService;
        protected INodeDataService NodeDataService =>
            _nodeDataService ?? (_nodeDataService = Container.Resolve<INodeDataService>());

        protected virtual INodeDataMapper GetNodeMapper(Node node)
        {
            var productName = node.ControlStrategy.Connector;
            var mapperName = AssemblyScanner.GetMapperClassFullNameByProductName(productName);
            return Container.ResolveNamed<object>(mapperName) as INodeDataMapper;
        }

        protected async Task EnsureNodeOnline()
        {
            var isPingable = await Node.IsPingable();

            if (!isPingable)
                throw new SmartHomeNodeOfflineException($"{Node.ToString()} is offline");
        }
    }
}
