using Autofac;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    public class MqttMessageProcessor : MessageProcessorBase<MqttMessageDto>
    {
        public MqttMessageProcessor(ILifetimeScope container) : base(container)
        {
        }

        public override async Task Process(MqttMessageDto message)
        {
            if (message is null
                || string.IsNullOrEmpty(message.Payload)
                || string.IsNullOrEmpty(message.Topic)
                || string.IsNullOrEmpty(message.ClientId)) return;

            var node = await NodeRepository.GetByClientIdAsync(message.ClientId);
            if (node is null) return;

            try
            {
                var handlerName = AssemblyScanner.GetHandlerClassFullNameByAssembly(node.ControlStrategy.ContractAssembly);
                var rawHandler = Container.ResolveNamed<object>(handlerName, new NamedParameter("node", node));

                if (!(rawHandler is IMessageHandler<MqttMessageDto> messageHandler))
                    throw new SmartHomeException($"Received message from clientId: {node.ClientId} but the resolver is not implemented");

                await messageHandler.Handle(message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "MQTT processing exception");
                throw;
            }
        }
    }
}
