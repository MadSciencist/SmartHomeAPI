﻿using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Contracts.TasmotaMqtt.Domain;
using SmartHome.Core.Abstractions;
using SmartHome.Core.Control;
using SmartHome.Core.Entities.Attributes;
using SmartHome.Core.Entities.ContractParams;
using SmartHome.Core.Entities.Entity;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt.Commands
{
    [DisplayText("Light")]
    [ParameterType(typeof(LightParam))]
    public class Light : MqttControlCommand, IControlCommand
    {
        public Light(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public async Task Execute(JObject commandParams)
        {
            var param = commandParams.ToObject<LightParam>().Validate();
            await EnsureNodeOnline();

            var logic = new LightLogic();
            var (topic, payload) = logic.GetTopicPayloadForLight(param);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{Node.BaseTopic}/{topic}")
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttBroker.PublishSystemMessageAsync(message);
        }
    }
}
