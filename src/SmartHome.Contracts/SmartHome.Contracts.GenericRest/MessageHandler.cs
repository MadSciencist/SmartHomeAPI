﻿using Autofac;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.MessageHanding;
using System.Collections.Generic;
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
            // TODO: instead of checking one by one, gather all of them and use NodeDataService.AddManyAsync
            foreach (KeyValuePair<string, JToken> token in message.Payload)
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

        private async Task ExtractSaveData(int nodeId, string magnitude, string value)
        {
            var property = await DataMapper.GetPhysicalPropertyByContractMagnitudeAsync(magnitude);

            // Check if there is associated system property
            if (property is null) return;

            var magnitudeDto = new NodeDataMagnitudeDto
            {
                Value = value,
                PhysicalProperty = property
            };

            await NodeDataService.AddSingleAsync(nodeId, magnitudeDto);

            NotificationService.PushDataNotification(nodeId, magnitudeDto);
        }
    }
}
