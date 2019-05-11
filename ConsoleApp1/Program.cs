using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;

namespace ConsoleApp1
{
    class Program
    {
        private readonly IMqttServer _mqttServer;

        public async Task Run()
        {
            var options = new MqttServerOptionsBuilder();
            options.WithDefaultEndpointPort(1883)
                .WithConnectionBacklog(100);

            await _mqttServer.StartAsync(options.Build());
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            _mqttServer = new MqttFactory().CreateMqttServer();
        }


    }
}
