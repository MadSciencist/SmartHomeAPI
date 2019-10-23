using Autofac;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.MessageHanding;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaMqtt
{
    public class Handler : MessageHandlerBase<MqttMessageDto>
    {
        public Handler(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public override async Task Handle(MqttMessageDto message)
        {
            // Espurna using json payload posts all data to /data topic
            if (message.Topic.Contains("/data"))
            {
                var payload = JObject.Parse(message.Payload);

                // TODO: Bulk processing
                foreach (var (magnitude, value) in payload)
                {
                    var mappedMagnitudeKey = DataMapper.GetMapping(magnitude);
                    if (!Node.ShouldMagnitudeBeStored(mappedMagnitudeKey)) continue;

                    await ProcessNodeMagnitude(magnitude, value.Value<string>());
                }
            }
        }
    }
}
