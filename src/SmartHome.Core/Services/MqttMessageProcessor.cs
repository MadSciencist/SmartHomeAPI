using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.MessageHandlers;

namespace SmartHome.Core.Services
{
    public class MqttMessageProcessor
    {
        private readonly ILifetimeScope _container;
        private readonly INodeRepository _nodeRepository;
        private readonly ILogger _logger;

        public MqttMessageProcessor(ILifetimeScope container, INodeRepository nodeRepository, ILoggerFactory loggerFactory)
        {
            _container = container;
            _nodeRepository = nodeRepository;
            _logger = loggerFactory.CreateLogger(typeof(MqttMessageProcessor));
        }

        public async Task ProcessMessage(MqttMessageDto message)
        {
            if (message == null) return;

            var node = await _nodeRepository.GetByClientIdAsync(message.ClientId);
            if (node is null) return;

            try
            {
                var handlerClass = $"{node.ControlStrategy.ContractAssembly}.Handlers.Handler";

                if (!(_container.ResolveNamed<object>(handlerClass) is IMqttMessageHandler messageHandler))
                    throw new SmartHomeException($"Received message from clientId: {node.ClientId} but the resolver is not implemented");

                await messageHandler.Handle(node, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT processing exception");
                throw;
            }
        }
    }
}
