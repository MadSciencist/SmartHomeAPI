using Autofac;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Domain;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Domain.Models;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHandlers;
using SmartHome.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Contracts.EspurnaMqtt.Handlers
{
    public class Handler : MessageHandlerBase<MqttMessageDto>, IMqttMessageHandler
    {
        public Handler(ILifetimeScope container) : base(container)
        {
        }

        public override async Task Handle(Node node, MqttMessageDto message)
        {
            // Espurna using json payload posts all data to /data topic
            if (message.Topic.Contains("/data"))
            {
                var payload = JObject.Parse(message.Payload);

                foreach (KeyValuePair<string, JToken> token in payload)
                {
                    // Check if current token is valid espurna sensor
                    if (Mappings.ValidProperties.Any(x => x.Magnitude == token.Key))
                    {
                        var sensorName = token.Key;
                        var sensorValue = token.Value.Value<string>();

                        // if the user wants to collect such sensor data
                        if (node.ControlStrategy.ControlStrategyLinkages
                            .Where(x => x.ControlStrategyLinkageTypeId == (int)LinkageType.Sensor)
                            .Any(x => x.InternalValue.CompareInvariant(sensorName)))
                        {
                            await ExtractSaveData(node.Id, sensorName, sensorValue);
                        }
                    }
                }
            }
        }

        private async Task ExtractSaveData(int nodeId, string sensorName, string value)
        {
            PhysicalProperty property = SystemMagnitudes.GetPhysicalPropertyByContextDictionary(Mappings.Map, sensorName);

            // Check if there is associated system value
            if (property != null)
            {
                await NodeDataService.AddSingleAsync(nodeId, EDataRequestReason.Node, new NodeDataMagnitudeDto
                {
                    Value = value,
                    PhysicalProperty = property
                });

                NotificationService.PushNodeDataNotification(nodeId, property, value);
            }
        }
    }
}
