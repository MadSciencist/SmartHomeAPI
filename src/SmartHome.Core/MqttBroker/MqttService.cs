using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Server;
using SmartHome.Core.MqttBroker.MessageHandling;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Server.Status;
using System;

namespace SmartHome.Core.MqttBroker
{
    public class MqttService : IMqttService
    {
        public IMqttServerOptions ServerOptions { get; set; }

        private readonly ILogger _logger;
        private readonly IMqttServer _mqttServer;
        private readonly MessageInterceptor _messageInterceptor;

        public MqttService(ILoggerFactory loggerFactory, MessageInterceptor interceptor)
        {
            _logger = loggerFactory.CreateLogger(typeof(MqttService));
            _mqttServer = new MqttFactory().CreateMqttServer();
            _logger.LogInformation("Mqtt broker created");
            _messageInterceptor = interceptor;
        }

        public async Task StartBroker()
        {
            await _mqttServer.StartAsync(ServerOptions);
            StartIntecepting();
            _logger.LogInformation($"Mqtt broker started on port: {ServerOptions.DefaultEndpointOptions.Port}");
        }

        public async Task StopBroker()
        {
            await _mqttServer.StartAsync(ServerOptions);
            _logger.LogInformation("Mqtt broker stopped");
        }

        public async Task<MqttClientPublishResult> PublishSystemMessageAsync(MqttApplicationMessage message)
        {
            return await _mqttServer.PublishAsync(message);
        }

        public async Task<ICollection<IMqttClientStatus>> GetClientStatusAsync()
        {
            return await _mqttServer.GetClientStatusAsync();
        }

        public void StartIntecepting()
        {
            try
            {

                _mqttServer.UseApplicationMessageReceivedHandler(async e =>
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
