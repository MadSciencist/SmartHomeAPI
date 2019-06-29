using Newtonsoft.Json.Linq;
using SmartHome.Core.Contracts.Mappings;
using SmartHome.Core.Domain;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Domain.Models;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHandlers;
using SmartHome.Core.Services;
using SmartHome.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Contracts.Mqtt.MessageHandling
{
    public class EspurnaJsonPayload : IMqttMessageHandler
    {
        private readonly INodeDataService _nodeDataService;
        private readonly NotificationService _notificationService;

        public EspurnaJsonPayload(INodeDataService nodeDataService, NotificationService notificationService)
        {
            _nodeDataService = nodeDataService;
            _notificationService = notificationService;
        }

        public async Task Handle(Node node, MqttMessageDto message)
        {
            // Espurna using json payload posts all data to /data topic
            if (message.Topic.Contains("/data"))
            {
                var payload = JObject.Parse(message.Payload);

                foreach (KeyValuePair<string, JToken> token in payload)
                {
                    // check if current token is valid espurna sensor
                    if (EspurnaMapping.ValidProperties.Any(x => x.Magnitude == token.Key))
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
            PhysicalProperty property = SystemMagnitudes.GetPhysicalPropertyByContextDictionary(EspurnaMapping.Map, sensorName);

            // Check if there is associated system value
            if (property != null)
            {
                await _nodeDataService.AddSingleAsync(nodeId, EDataRequestReason.Node, new NodeDataMagnitudeDto
                {
                    Value = value,
                    PhysicalProperty = property
                });

                _notificationService.PushNodeDataNotification(nodeId, property, value);
            }
        }
    }
}
