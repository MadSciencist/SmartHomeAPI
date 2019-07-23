using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using SmartHomeMock.SensorMock.Domain.Interfaces;
using SmartHomeMock.SensorMock.Entities.Data;

namespace SmartHomeMock.SensorMock.Domain
{
    public class EspurnaModuleMock : IModuleMock
    {
        private IMqttFactory _mqttFactory;
        private IMqttClient _client;

        private Task _task;

        private ISensorMock[] _sensors;

        public EspurnaModuleMock(IMqttFactory mqttFactory)
        {
            _mqttFactory = mqttFactory;

            _client = _mqttFactory.CreateMqttClient();

            foreach (var sensorMock in _sensors)
            {
                sensorMock.DataReceived += OnSensorDataUpdated;
            }
        }

        public Task Start()
        {
            _client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("my/topic").Build());

                Console.WriteLine("### SUBSCRIBED ###");
            });

            _client.UseApplicationMessageReceivedHandler(HandleMessage);

            return null;
        }

        

        private Task HandleMessage(MqttApplicationMessageReceivedEventArgs x)
        {
            // Zmien swoj stan 
            _sensors[0].UpdateState();
            return null;
        }
        // Subscribe to MQTT - to be able handling requests like change relay state

        // Subscribe to Sensors events to add/public messages to MQTT
        // API

        private void OnSensorDataUpdated(object sender, SensorData data)
        {
            // generate message
            //_client.PublishAsync()
        }

    }
}