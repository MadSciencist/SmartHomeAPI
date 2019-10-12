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
        public RgbLight(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        public async Task Execute(JObject commandParams)
        {
            var param = commandParams.ToObject<RgbLightParam>().Validate();
            await EnsureNodeOnline();

            var logic = new LightLogic();
            var (topic, payload) = logic.GetTopicPayloadForRgbLight(param);

            // TODO abstract await the MQTTNet message builder
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
