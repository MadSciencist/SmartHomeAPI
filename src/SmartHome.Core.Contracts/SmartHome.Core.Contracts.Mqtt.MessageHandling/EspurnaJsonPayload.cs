﻿using Newtonsoft.Json.Linq;
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
    public class EspurnaJsonPayload : IMqttMessageHandler
    {
        private readonly INodeDataService _nodeDataService;

        public EspurnaJsonPayload(INodeDataService nodeDataService)
        {
            _nodeDataService = nodeDataService;
        }

        public async Task Handle(Node node, MqttMessageDto message)
        {
            if (message.Topic.Contains("/data"))
            {
                var payload = JObject.Parse(message.Payload);

                foreach (KeyValuePair<string, JToken> token in payload)
                {
                    // check if current token is valid espurna sensor
                    if (Espurna.ValidEspurnaSensors.Any(x => x.Magnitude == token.Key))
                    {
                        var sensorName = token.Key;
                        var sensorValue = token.Value.Value<string>();

                        // if the user wants to save such sensor data
                        if (node.ControlStrategy.RegisteredSensors.Any(x => string.Compare(x.Name, sensorName, StringComparison.InvariantCultureIgnoreCase) == 0))
                        {
                            await ExtractSaveData(sensorName, sensorValue);
                        }
                    }
                }

            }
        }

        private async Task ExtractSaveData(string sensorName, string value)
        {
            var unit = Espurna.ValidEspurnaSensors.First(x => string.Compare(x.Magnitude, sensorName, StringComparison.InvariantCultureIgnoreCase) == 0).Unit;

            await _nodeDataService.AddSingleAsync(EDataRequestReason.Node, new NodeDataDto
            {
                Value = value,
                Magnitude = sensorName,
                Unit = unit
            });
        }
    }
}
