using Autofac;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHanding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaMqtt.Handlers
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

                // TODO: instead of checking one by one, gather all of them and use NodeDataService.AddManyAsync
                foreach (KeyValuePair<string, JToken> token in payload)
                {
                    // Check if current token is valid espurna sensor
                    if (base.DataMapper.IsPropertyValid(token.Key))
                    {
                        var sensorName = token.Key;
                        var sensorValue = token.Value.Value<string>();

                        await ExtractSaveData(base.Node.Id, sensorName, sensorValue);
                    }
                }
            }
        }

        private async Task ExtractSaveData(int nodeId, string magnitude, string value)
        {
            var property = base.DataMapper.GetPhysicalPropertyByContractMagnitude(magnitude);

            // Check if there is associated system property
            if (property is null) return;

            await NodeDataService.AddSingleAsync(nodeId, EDataRequestReason.Node, new NodeDataMagnitudeDto
            {
                Value = value,
                PhysicalProperty = property
            });

            NotificationService.PushNodeDataNotification(nodeId, property, value);
        }
    }
}
