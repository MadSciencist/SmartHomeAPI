using SmartHome.Core.Domain;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Core.MessageHandlers;
using SmartHome.Core.Services;

namespace SmartHome.Core.Contracts.Mqtt.MessageHandling
{
    public class Espurna : IMqttMessageHandler
    {
        private readonly INodeDataService _nodeDataService;

        public Espurna(INodeDataService nodeDataService)
        {
            _nodeDataService = nodeDataService;
        }

        public async Task Handle(Node node, MqttMessageDto message)
        {
            // check if topic contains valid espurna sensor element
            foreach (var validSensorName in ValidSensors.Select(x => x.Magnitude))
            {
                // if the topic contains valid sensor for espurna
                if (message.Topic.Contains(validSensorName))
                {
                    // if the user wants to save such sensor data
                    if (node.ControlStrategy.RegisteredSensors.Any(x => string.Compare(x.Name, validSensorName, StringComparison.InvariantCultureIgnoreCase) == 0))
                        await ExtractSaveData(message);
                }
            }
        }

        private async Task ExtractSaveData(MqttMessageDto message)
        {
            var magnitude = message.Topic.Split("/").Last();
            var unit = ValidSensors.First(x => string.Compare(x.Magnitude, magnitude, StringComparison.InvariantCultureIgnoreCase) == 0).Unit;

            await _nodeDataService.AddSingleAsync(EDataRequestReason.Node, new NodeDataDto
            {
                Value = message.Payload,
                Magnitude = magnitude,
                Unit = unit
            });
        }

        // https://github.com/xoseperez/espurna/wiki/MQTT
        public static readonly ICollection<PhysicalValue> ValidSensors = new List<PhysicalValue>
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
        };
    }
}
