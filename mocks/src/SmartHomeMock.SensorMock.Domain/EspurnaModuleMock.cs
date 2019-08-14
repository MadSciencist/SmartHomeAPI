using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Configuration;
using SmartHomeMock.SensorMock.Entities.Data;
using SmartHomeMock.SensorMock.Entities.Enums;
using SmartHomeMock.SensorMock.Infrastructure.Interfaces;

namespace SmartHomeMock.SensorMock.Domain
{
    public class EspurnaModuleMock : IModuleMock
    {
        private readonly IFactory<ESensorType, ISensorMock> _sensorMockFactory;
        private readonly IMqttFactory _mqttFactory;

        private IMqttClient _client;
        private IMqttClientOptions _options;

        private Module _module;
        private Broker _broker;

        private ISensorMock[] _sensors;

        public EspurnaModuleMock(IFactory<ESensorType, ISensorMock> sensorMockFactory, IMqttFactory mqttFactory)
        {
            _sensorMockFactory = sensorMockFactory;
            _mqttFactory = mqttFactory;
        }

        public void Initialize(Module module, Broker broker)
        {
            _module = module;
            _broker = broker;

            _options = new MqttClientOptionsBuilder()
                .WithClientId(_module.ClientId)
                .WithTcpServer(_broker.Host, broker.Port)
                .Build();

            _client = _mqttFactory.CreateMqttClient();

            _sensors = _module.Sensors.Select(InitializeSensorMock).ToArray();
        }

        public void Start()
        {
            _client.ConnectAsync(_options).Wait();

            foreach (var sensorMock in _sensors)
            {
                sensorMock.StateChanged += OnSensorStateChanged;
            }

            _client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await _client.SubscribeAsync(new TopicFilterBuilder().Build());

                Console.WriteLine("### SUBSCRIBED ###");
            });

            _client.UseApplicationMessageReceivedHandler(HandleMessage);

            foreach (var sensorMock in _sensors)
            {
                StartSensorMock(sensorMock);
            }
        }

        private ISensorMock InitializeSensorMock(Sensor sensor)
        {
            var sensorMock = _sensorMockFactory.Get(sensor.Type);

            sensorMock.Initialize(sensor);

            return sensorMock;
        }

        private void StartSensorMock(ISensorMock sensorMock)
        {
            sensorMock.Start();
        }

        private Task HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            // Zmien swoj stan 
            //_sensors[0].UpdateState(new SensorData());
            return null;


        }
        // Subscribe to MQTT - to be able handling requests like change relay state

        // Subscribe to Sensors events to add/public messages to MQTT
        // API

        private void OnSensorStateChanged(object sender, SensorData data)
        {
            // generate message

            var message = new MqttApplicationMessageBuilder().
                WithTopic("").
                Build();

            //_client.PublishAsync().Wait();
        }
    }
}