using Autofac;
using SmartHome.Core.Entities.Abstractions;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Services;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    public abstract class MessageHandlerBase<T> : IMessageHandler<T> where T : class, new()
    {
        protected Node Node { get; }

        protected ILifetimeScope Container { get; }

        private INodeDataService _nodeDataService;
        protected INodeDataService NodeDataService =>
            _nodeDataService ?? (_nodeDataService = Container.Resolve<INodeDataService>());

        private NotificationService _notificationService;
        protected NotificationService NotificationService =>
            _notificationService ?? (_notificationService = Container.Resolve<NotificationService>());

        private INodeDataMapper _nodeDataMapper;
        protected INodeDataMapper DataMapper
        {
            get
            {
                if (_nodeDataMapper is null)
                {
                    var productName = Node.ControlStrategy.AssemblyProduct;
                    var mapperName = AssemblyScanner.GetMapperClassFullNameByProductName(productName);
                    _nodeDataMapper = Container.ResolveNamed<object>(mapperName) as INodeDataMapper;
                }

                return _nodeDataMapper;
            }
        }

        protected string ApplyConversion(string magnitude, string value)
        {
            var converterType = DataMapper.GetConverter(magnitude);
            var converter = Activator.CreateInstance(converterType) as IDataConverter;
            return converter?.Convert(value);
        }

        #region ctor
        protected MessageHandlerBase(ILifetimeScope container, Node node)
        {
            Node = node;
            Container = container;
        }
        #endregion

        #region IMessageHandler<T> impl
        public abstract Task Handle(T message);
        #endregion
    }
}
