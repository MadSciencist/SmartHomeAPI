using Microsoft.Extensions.Diagnostics.HealthChecks;
using MQTTnet;
using SmartHome.Core.MqttBroker;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.API.Extensions
{
    public class MqttBrokerHealthCheck : IHealthCheck
    {
        private readonly IMqttBroker _broker;

        public MqttBrokerHealthCheck(IMqttBroker broker)
        {
            _broker = broker;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _broker.PublishSystemMessageAsync(new MqttApplicationMessageBuilder()
                    .WithTopic("health")
                    .WithPayload("")
                    .Build());

                return HealthCheckResult.Healthy("MQTT Broker is healthy");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(ex.Message);
            }
        }
    }
}
