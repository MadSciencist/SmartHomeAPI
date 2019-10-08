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
    [DisplayText("RGB Light")]
    [ParameterType(typeof(RgbLightParam))]
    public class RgbLight : MqttControlCommand, IControlCommand
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
