using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Server;
using MQTTnet.Server.Status;
using SmartHome.Core.MqttBroker.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.MqttBroker
{
    public class MqttService : IMqttService, IHostedService
    {
        public IMqttServer Server { get; private set; }
        public IMqttServerOptions Options { get; private set; }

        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly MessageInterceptor _messageInterceptor;

        public MqttService(ILoggerFactory loggerFactory, MessageInterceptor interceptor, IConfiguration config)
        {
            _config = config;
            _messageInterceptor = interceptor;
            _logger = loggerFactory.CreateLogger(typeof(MqttService));

            Options = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(_config.GetValue<int>("MqttBroker:Port"))
                .WithConnectionBacklog(_config.GetValue<int>("MqttBroker:MaxBacklog"))
                .WithClientId(_config.GetValue<string>("MqttBroker:ClientId"))
                .Build();

            Server = new MqttFactory().CreateMqttServer();
            _logger.LogInformation("Mqtt broker created");
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            await Server.StartAsync(Options);
            StartIntecepting();
            _logger.LogInformation($"Mqtt broker started on port: {Options.DefaultEndpointOptions.Port}");
        }

        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "MQTT Broker exception");
                throw;
            }
        }
    }
}
