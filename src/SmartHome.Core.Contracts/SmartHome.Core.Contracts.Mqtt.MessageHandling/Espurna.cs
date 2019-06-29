using Autofac;
using SmartHome.Core.Contracts.Mappings;
using SmartHome.Core.Domain;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Domain.Models;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHandlers;
using SmartHome.Core.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Contracts.Mqtt.MessageHandling
{
    public class Espurna : MessageHandlerBase<MqttMessageDto>, IMqttMessageHandler
    {
        public Espurna(ILifetimeScope container) : base(container)
        {
        }

        public override async Task Handle(Node node, MqttMessageDto message)
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
            string magnitude;

            // Trick to get magnitudes in form 'relay/0'
            if (message.Topic.Contains("relay"))
            {
                var spliced = message.Topic.Split("/");
                var beforeLast = spliced[spliced.Count() - 2];
                magnitude = $"{beforeLast}/${spliced.Last()}";
            }
            else
            {
                magnitude = message.Topic.Split("/").Last();
            }

            PhysicalProperty property = SystemMagnitudes.GetPhysicalPropertyByContextDictionary(EspurnaMapping.Map, magnitude);

            // Check if there is associated system value
            if (property != null)
            {
                await NodeDataService.AddSingleAsync(nodeId, EDataRequestReason.Node, new NodeDataMagnitudeDto
                {
                    Value = message.Payload,
                    PhysicalProperty = property
                });

                NotificationService.PushNodeDataNotification(nodeId, property, message.Payload);
            }
        }
    }
}
