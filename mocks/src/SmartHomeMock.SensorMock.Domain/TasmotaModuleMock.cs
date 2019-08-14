using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Configuration;
using SmartHomeMock.SensorMock.Entities.Data;
using SmartHomeMock.SensorMock.Entities.Enums;
using SmartHomeMock.SensorMock.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHomeMock.SensorMock.Domain
{
    public class TasmotaModuleMock : ModuleMockBase
    {
        private readonly IMqttFactory _mqttFactory;

        private IMqttClient _client;
        private IMqttClientOptions _options;

        public TasmotaModuleMock(IFactory<ESensorType, ISensorMock> sensorMockFactory, IMqttFactory mqttFactory)
            : base(sensorMockFactory)
        {
            _mqttFactory = mqttFactory;
        }

        public override void Initialize(Module module, Broker broker)
        {
            Module = module;
            Broker = broker;

            _options = new MqttClientOptionsBuilder()
                .WithClientId(Module.ClientId)
                .WithTcpServer(Broker.Host, broker.Port)
                .Build();

            _client = _mqttFactory.CreateMqttClient();

            Sensors = Module.Sensors.Select(InitializeSensorMock).ToArray();
        }

        public override void Start()
        {
            _client.ConnectAsync(_options).Wait();

            foreach (var sensorMock in Sensors)
            {
                sensorMock.StateChanged += OnSensorStateChanged;
            }

            _client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                var topics = GetTopics();
                await _client.SubscribeAsync(topics);

                Console.WriteLine("### SUBSCRIBED ###");
            });

            _client.UseApplicationMessageReceivedHandler(HandleMessage);

            foreach (var sensorMock in Sensors)
            {
                StartSensorMock(sensorMock);
            }
        }

        private void HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ClientId != Module.ClientId)
            {

            }

            var listener = Module.Listeners.Single(l => l.Topic == e.ApplicationMessage.Topic);
            var sensor = Sensors.Single(s => s.Id == listener.SensorId);

            var data = new SensorData
            {
                Values = new []{ Encoding.UTF8.GetString(e.ApplicationMessage.Payload) },
                Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload)
            };

            sensor.UpdateState(data);
        }

        private TopicFilter[] GetTopics()
        {
            return Module.Listeners.Select(l => GetTopic(l.Topic)).ToArray();
        }

        private TopicFilter GetTopic(string topic)
        {
            return new TopicFilterBuilder().WithTopic(topic).Build();
        }

        protected override void OnSensorStateChanged(object sender, SensorData data)
        {
            var message = new MqttApplicationMessageBuilder().
                WithTopic(data.Sensor.Topic).
                WithPayload(!string.IsNullOrWhiteSpace(data.Payload)
                    ? data.Payload
                    : string.Format(data.Sensor.Payload, data.Values.Select(v => v as object).ToArray())).
                Build();

            _client.PublishAsync(message, CancellationToken.None).Wait();
        }
    }
}
