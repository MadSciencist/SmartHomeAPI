using Autofac;
using SmartHome.Core.MqttBroker;

namespace SmartHome.Core.Control
{
    public class MqttControlStrategyBase : ControlStrategyBase
    {
        protected MqttControlStrategyBase(ILifetimeScope container) : base(container)
        {
        }

        private IMqttBroker _mqttBroker;
        protected IMqttBroker MqttBroker =>
            _mqttBroker ?? (_mqttBroker = Container.Resolve<IMqttBroker>());
    }
}
