using Autofac;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto.NodeData;
using System;
using System.Threading.Tasks;
using SmartHome.Core.MessageHanding;

namespace SmartHome.Contracts.EspurnaRest.Handlers
{
    public class Handler : MessageHandlerBase<RestMessageDto>, IMessageHandler<RestMessageDto>
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
