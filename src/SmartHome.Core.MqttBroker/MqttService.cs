using System;
using System.Text;
using MQTTnet;
using MQTTnet.Server;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet.Client.Publishing;
using Newtonsoft.Json;

namespace SmartHome.Core.MqttBroker
{
    public class MqttService : IMqttService
    {
        private readonly ILogger _logger;
        public IMqttServerOptions ServerOptions { get; set; }
        private readonly IMqttServer _mqttServer;

        public MqttService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(MqttService));
            _mqttServer = new MqttFactory().CreateMqttServer();
            _logger.LogInformation("Mqtt broker created");
        }

        public async Task Start()
        {
            await _mqttServer.StartAsync(ServerOptions);
            _logger.LogInformation("Mqtt broker started");
        }

        public async Task Stop()
        {
            await _mqttServer.StartAsync(ServerOptions);
            _logger.LogInformation("Mqtt broker stopped");
        }

        public async Task<MqttClientPublishResult> PublishSystemMessageAsync(MqttApplicationMessage message)
        {
            return await _mqttServer.PublishAsync(message);
        }

        public void Log()
        {
            _mqttServer.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ ClientID = {e.ClientId}");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });
        }
    }
}
