using System.Threading.Tasks;
using Autofac;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Services;

namespace SmartHome.Core.MessageHanding
{
    public abstract class MessageHandlerBase<T> : IMessageHandler<T> where T: class, new() 
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


        private INodeDataMapper _nodeDataMapper;
        protected INodeDataMapper GetDataMapper(Node node)
        {
            if (_nodeDataMapper is null)
            {
                var fullname = AssemblyScanner.GetMapperClassFullNameByAssembly(node.ControlStrategy.ContractAssembly);

                _nodeDataMapper = Container.ResolveNamed<object>(fullname) as INodeDataMapper;
            }

            return _nodeDataMapper;
        }
    }
}
