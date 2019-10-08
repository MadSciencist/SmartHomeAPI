using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Contracts.TasmotaMqtt.Domain;
using SmartHome.Core.Control;
using SmartHome.Core.Entities.ContractParams;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Infrastructure.Attributes;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt.Commands
{
    [DisplayText("Light")]
    [ParameterType(typeof(LightParam))]
    public class Light : MqttControlCommand, IControlCommand
    {
        public Light(ILifetimeScope container) : base(container)
        {
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var param = commandParams.ToObject<LightParam>().Validate();

            var logic = new LightLogic();
            var (topic, payload) = logic.GetTopicPayloadForLight(param);

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
