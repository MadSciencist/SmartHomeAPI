using Autofac;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.MessageHanding;
using System;
using System.Threading.Tasks;

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
