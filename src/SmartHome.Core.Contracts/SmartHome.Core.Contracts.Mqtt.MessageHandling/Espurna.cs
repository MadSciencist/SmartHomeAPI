using SmartHome.Core.Contracts.Mappings;
using SmartHome.Core.Domain;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Domain.Models;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHandlers;
using SmartHome.Core.Services;
using SmartHome.Core.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Contracts.Mqtt.MessageHandling
{
    public class Espurna : IMqttMessageHandler
    {
        private readonly INodeDataService _nodeDataService;
        private readonly NotificationService _notificationService;

        public Espurna(INodeDataService nodeDataService, NotificationService notificationService)
        {
            _nodeDataService = nodeDataService;
            _notificationService = notificationService; // TODO base class from notificatioBase
        }

        public async Task Handle(Node node, MqttMessageDto message)
        {
            // check if topic contains valid espurna sensor element
            foreach (var validSensorName in EspurnaMapping.ValidProperties.Select(x => x.Magnitude))
            {
                // if the topic contains valid sensor for espurna
                if (message.Topic.Contains(validSensorName))
                {
                    // if the user wants to collect such sensor data
                    if (node.ControlStrategy.ControlStrategyLinkages
                        .Where(x => x.ControlStrategyLinkageTypeId == (int) LinkageType.Sensor)
                        .Any(x => x.InternalValue.CompareInvariant(validSensorName)))
                    {
                        await ExtractSaveData(node.Id, message);
                    }
                }
            }
        }

        private async Task ExtractSaveData(int nodeId, MqttMessageDto message)
        {
            // TODO relay/o causes bug when its in topic
            var magnitude = message.Topic.Split("/").Last();

            PhysicalProperty property = SystemMagnitudes.GetPhysicalPropertyByContextDictionary(EspurnaMapping.Map, magnitude);

            // Check if there is associated system value
            if (property != null)
            {
                await _nodeDataService.AddSingleAsync(nodeId, EDataRequestReason.Node, new NodeDataMagnitudeDto
                {
                    Value = message.Payload,
                    PhysicalProperty = property
                });

                _notificationService.PushNodeDataNotification(nodeId, property, message.Payload);
            }
        }
    }
}
