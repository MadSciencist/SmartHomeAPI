using Autofac;
using SmartHome.Core.Entities.Abstractions;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Services;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Threading.Tasks;
using SmartHome.Core.Dto;

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
            if (converterType is null) return value; // no converter required short circuit

            var converter = Activator.CreateInstance(converterType) as IDataConverter;
            return converter?.Convert(value);
        }

        /// <summary>
        /// Saves the data in DB and notifies the client via SignalR.
        /// </summary>
        /// <param name="magnitude">Mapped magnitude</param>
        /// <param name="value">Value of magnitude</param>
        /// <returns></returns>
        protected async Task ProcessNodeMagnitude(string magnitude, string value)
        {
            var magnitudeDto = await BuildNodeDto(magnitude, value);

            await NodeDataService.AddSingleAsync(Node.Id, magnitudeDto);
            NotificationService.PushDataNotification(Node.Id, magnitudeDto);
        }

        private async Task<NodeDataDto> BuildNodeDto(string magnitude, string value)
        {
            var property = await DataMapper.GetPhysicalPropertyByMagnitudeAsync(magnitude);

            // Check if there is associated system property
            if (property is null)
                throw new InvalidOperationException($"{magnitude} does not maps to existing PhysicalProperty");

            return new NodeDataDto
            {
                Value = ApplyConversion(magnitude, value),
                PhysicalProperty = property
            };
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
