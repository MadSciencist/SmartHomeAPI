using Autofac;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    public class RestMessageProcessor : MessageProcessorBase<RestMessageDto>
    {
        public RestMessageProcessor(ILifetimeScope container) : base(container)
        {
        }

        public override async Task Process(RestMessageDto message)
        {
            if (message?.Payload is null || string.IsNullOrEmpty(message?.ClientId)) return;

            var node = await NodeRepository.GetByClientIdAsync(message.ClientId);
            if (node is null) return;

            try
            {
                var handlerName = AssemblyScanner.GetHandlerClassFullNameByProductName(node.ControlStrategy.Connector);
                var rawHandler = Container.ResolveNamed<object>(handlerName, new NamedParameter("node", node));

                if (!(rawHandler is IMessageHandler<RestMessageDto> messageHandler))
                    throw new SmartHomeException($"Received message from clientId: {node.ClientId} but the resolver is not implemented");

                await messageHandler.Handle(message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "REST message processing exception");
                throw;
            }
        }
    }
}
