using Autofac;
using SmartHome.Core.BusinessLogic.TopicResolvers;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.BusinessLogic
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
            if(message == null) return;

            var node = await _nodeRepository.GetByClientIdAsync(message.ClientId);
            if(node == null)
                throw new SmartHomeException("Received message with invalid ClientId");

            if (string.Compare(node.ControlStrategy.ProviderName, "Mqtt", StringComparison.InvariantCultureIgnoreCase) != 0)
                throw new SmartHomeException($"Received message from clientId: {node.ClientId} with invalid control strategy");

            var resolverClassName = $"SmartHome.Core.BusinessLogic.TopicResolvers.{node.ControlStrategy.ContextName}";

            if (!(_container.ResolveNamed<object>(resolverClassName) is ITopicResolver topicResolver))
                throw new SmartHomeException($"Received message from clientId: {node.ClientId} but the resolver is not implemented");

            await topicResolver.Resolve(node, message);
        }
    }
}
