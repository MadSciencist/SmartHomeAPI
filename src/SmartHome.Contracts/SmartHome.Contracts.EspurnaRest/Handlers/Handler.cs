using Autofac;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto.NodeData;
using SmartHome.Core.MessageHandlers;
using System;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaRest.Handlers
{
    public class Handler : MessageHandlerBase<RestMessageDto>, IRestMessageHandler
    {
        public Handler(ILifetimeScope container) : base(container)
        {
        }

        public override Task Handle(Node node, RestMessageDto message)
        {
            throw new NotImplementedException();
        }
    }
}
