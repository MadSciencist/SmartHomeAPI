using Autofac;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto.NodeData;
using System;
using System.Threading.Tasks;
using SmartHome.Core.MessageHanding;

namespace SmartHome.Contracts.EspurnaRest.Handlers
{
    public class Handler : MessageHandlerBase<RestMessageDto>
    {
        public Handler(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public override Task Handle(RestMessageDto message)
        {
            throw new NotImplementedException();
        }
    }
}
