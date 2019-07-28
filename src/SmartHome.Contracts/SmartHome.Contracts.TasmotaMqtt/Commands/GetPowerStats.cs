using Autofac;
using MQTTnet;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Control;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure.Attributes;
using System.Threading.Tasks;

namespace SmartHome.Contracts.TasmotaMqtt.Commands
{
    [DisplayText("Get power statistics")]
    public class GetPowerStats : MqttControlStrategyBase, IControlCommand
    {
        public GetPowerStats(ILifetimeScope container) : base(container) 
        {
        }

        public async Task Execute(Node node, JObject commandParams)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"{node.BaseTopic}/status")
                .WithPayload("8")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttBroker.PublishSystemMessageAsync(message);
        }
    }
}
