using SmartHome.Core.Domain;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHandlers;
using SmartHome.Core.Services;
using System;
using System.Collections.Generic;
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
            foreach (var validSensorName in ValidEspurnaSensors.Select(x => x.Magnitude))
            {
                // if the topic contains valid sensor for espurna
                if (message.Topic.Contains(validSensorName))
                {
                    // if the user wants to collect such sensor data
                    if (node.ControlStrategy.ControlStrategyLinkages
                        .Where(x => x.ControlStrategyLinkageTypeId == (int)LinkageType.Sensor)
                        .Any(x => string.Compare(x.InternalValue, validSensorName, StringComparison.InvariantCultureIgnoreCase) == 0))
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
            var unit = ValidEspurnaSensors.First(x => string.Compare(x.Magnitude, magnitude, StringComparison.InvariantCultureIgnoreCase) == 0).Unit;

            await _nodeDataService.AddSingleAsync(nodeId, Domain.Enums.DataRequestReason.Node, new NodeDataMagnitudeDto
            {
                Value = message.Payload,
                Magnitude = magnitude,
                Unit = unit
            });

            _notificationService.AddNotification(nodeId, magnitude, message.Payload);
        }

        // https://github.com/xoseperez/espurna/wiki/MQTT
        public static readonly ICollection<PhysicalValue> ValidEspurnaSensors = new List<PhysicalValue>
        {
            new PhysicalValue("temperature", "C"),
            new PhysicalValue("humidity", "%"),
            new PhysicalValue("pressure", "hPa"),
            new PhysicalValue("current", "A"),
            new PhysicalValue("voltage", "V"),
            new PhysicalValue("power", "W"),
            new PhysicalValue("apparent", "W"),
            new PhysicalValue("reactive", "W"),
            new PhysicalValue("factor", "%"),
            new PhysicalValue("energy", "kWh"),
            new PhysicalValue("energy_delta", "kWh"),
            new PhysicalValue("analog", "bit"),
            new PhysicalValue("digital", "binary"),
            new PhysicalValue("event", ""),
            new PhysicalValue("pm1dot0", "ppm"),
            new PhysicalValue("pm1dot5", "ppm"),
            new PhysicalValue("pm10", "ppm"),
            new PhysicalValue("co2", "ppm"),
            new PhysicalValue("lux", "lux"),
            new PhysicalValue("distance", "m"),
            new PhysicalValue("hcho", "ppm"),
            new PhysicalValue("ldr_cpm", "events"),
            new PhysicalValue("ldr_uSvh", "mSv"),
            new PhysicalValue("count", "events"),
            new PhysicalValue("relay/0", "bit"),
            new PhysicalValue("relay/1", "bit"),
            new PhysicalValue("relay/2", "bit"),
            new PhysicalValue("relay/3", "bit"),
        };
    }
}
