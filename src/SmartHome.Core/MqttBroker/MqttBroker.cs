using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Server;
using MQTTnet.Server.Status;
using SmartHome.Core.MqttBroker.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Core.MqttBroker
{
    public class MqttBroker : IMqttBroker
    {
        public IMqttServerOptions ServerOptions { get; set; }
        public IMqttServer Server { get; private set; }
        public IMqttServerOptions Options { get; private set; }

        private readonly ILogger _logger;
        private readonly MessageInterceptor _messageInterceptor;

        public MqttBroker(ILogger<MqttBroker> logger, MessageInterceptor interceptor, IConfiguration config)
        {
            _messageInterceptor = interceptor;
            _logger = logger;

            Options = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(config.GetValue<int>("MqttBroker:Port"))
                .WithConnectionBacklog(config.GetValue<int>("MqttBroker:MaxBacklog"))
                .WithClientId(config.GetValue<string>("MqttBroker:ClientId"))
                .Build();

            Server = new MqttFactory().CreateMqttServer();
            _logger.LogInformation("Mqtt broker created");
        }

        public async Task StartAsync()
        {
            await Server.StartAsync(Options);
            StartIntecepting();
            _logger.LogInformation($"Mqtt broker started on port: {Options.DefaultEndpointOptions.Port}");
        }

        public async Task StopAsync()
        {
            await Server.StopAsync();
            _logger.LogInformation("Mqtt broker stopped");
        }

        public async Task<MqttClientPublishResult> PublishSystemMessageAsync(MqttApplicationMessage message)
        {
            return await Server.PublishAsync(message);
        }

        public async Task<ICollection<IMqttClientStatus>> GetClientStatusAsync()
        {
            return await Server.GetClientStatusAsync();
        }

        public void StartIntecepting()
        {
            try
            {
                Server.UseApplicationMessageReceivedHandler(async e =>
                {
                    await _messageInterceptor.Intercept(new ReceivedMessage
                    {
                        ClientId = e.ClientId,
                        Topic = e.ApplicationMessage.Topic,
                        ContentType = e.ApplicationMessage.ContentType,
                        Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload)
                    });
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT Broker exception");
                throw;
            }
        }
    }
}
