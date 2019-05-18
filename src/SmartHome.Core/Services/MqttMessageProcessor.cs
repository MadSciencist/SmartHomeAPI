using System;
using System.Threading.Tasks;
using Autofac;
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

        public MqttMessageProcessor(ILifetimeScope container, INodeRepository nodeRepository)
        {
            _container = container;
            _nodeRepository = nodeRepository;
        }

        public async Task ProcessMessage(MqttMessageDto message)
        {
            if (message == null) return;

            var node = await _nodeRepository.GetByClientIdAsync(message.ClientId);
            if (node == null) return;

            if (string.Compare(node.ControlStrategy.ProviderName, "Mqtt", StringComparison.InvariantCultureIgnoreCase) != 0)
                throw new SmartHomeException($"Received message from clientId: {node.ClientId} with invalid control strategy");

            var resolverClassName = $"SmartHome.Core.Contracts.Mqtt.MessageHandling.{node.ControlStrategy.MessageReceiveContext}";

            if (!(_container.ResolveNamed<object>(resolverClassName) is IMqttMessageHandler topicResolver))
                throw new SmartHomeException($"Received message from clientId: {node.ClientId} but the resolver is not implemented");

            await topicResolver.Handle(node, message);
        }
    }
}
