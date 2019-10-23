using Autofac;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.MessageHanding;
using System.Threading.Tasks;

namespace SmartHome.Contracts.GenericRest
{
    /// <summary>
    /// This class handles all messages
    /// It assumes that payload of message is in JSON format
    /// </summary>
    public class Handler : MessageHandlerBase<RestMessageDto>
    {
        public Handler(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public override async Task Handle(RestMessageDto message)
        {
            // TODO: Bulk processing
            foreach (var (magnitude, value) in message.Payload)
            {
                var mappedMagnitudeKey = DataMapper.GetMapping(magnitude);
                if (!Node.ShouldMagnitudeBeStored(mappedMagnitudeKey)) continue;

                await ProcessNodeMagnitude(magnitude, value.Value<string>());
            }
        }
    }
}
