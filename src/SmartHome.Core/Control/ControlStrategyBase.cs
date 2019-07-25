﻿using Autofac;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.Services;

namespace SmartHome.Core.Control
{
    public abstract class ControlStrategyBase
    {
        protected ILifetimeScope Container { get; }

        protected ControlStrategyBase(ILifetimeScope container)
        {
            Container = container;
        }
        
        private NotificationService _notificationService;
        protected NotificationService NotificationService =>
            _notificationService ?? (_notificationService = Container.Resolve<NotificationService>());

        private INodeDataService _nodeDataService;
        protected INodeDataService NodeDataService =>
            _nodeDataService ?? (_nodeDataService = Container.Resolve<INodeDataService>());

        protected virtual INodeDataMapper GetNodeMapper(Node node)
        {
            var productName = node.ControlStrategy.AssemblyProduct;
            var mapperName = AssemblyScanner.GetMapperClassFullNameByProductName(productName);
            return Container.ResolveNamed<object>(mapperName) as INodeDataMapper;
        }
    }
}