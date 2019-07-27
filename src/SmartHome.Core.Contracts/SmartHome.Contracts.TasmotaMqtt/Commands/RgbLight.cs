﻿using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Contracts.TasmotaMqtt.Domain;
using SmartHome.Core.Control;
using SmartHome.Core.Domain.ContractParams;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure.Attributes;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt.Commands
{
    [DisplayText("RGB Light")]
    public class RgbLight : MqttControlStrategyBase, IControlCommand
    {
        public RgbLight(ILifetimeScope container) : base(container) 
        {
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var param = commandParams.ToObject<RgbLightParam>();

            var logic = new LightLogic();
            var (topic, payload) = logic.GetTopicPayloadForRgbLight(param);
            
            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{node.BaseTopic}/{topic}")
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttBroker.PublishSystemMessageAsync(message);
        }
    }
}
