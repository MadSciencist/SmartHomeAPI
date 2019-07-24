using Autofac;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto.NodeData;
using System;
using System.Threading.Tasks;
using SmartHome.Core.MessageHanding;

namespace SmartHome.Contracts.EspurnaRest
{
    public class Handler : MessageHandlerBase<RestMessageDto>
    {
        public Handler(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        // Espurna REST module is synchronous - message handling is done separately by each command
        // This should be never invoked, needed by framework
        public override Task Handle(RestMessageDto message)
        {
            throw new NotImplementedException();
        }
    }
}
