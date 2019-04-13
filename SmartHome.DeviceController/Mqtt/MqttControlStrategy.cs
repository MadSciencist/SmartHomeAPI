using System;

namespace SmartHome.DeviceController.Mqtt
{
    public class MqttControlStrategy : IControlStrategy
    {
        public void Execute()
        {
            Console.WriteLine(nameof(MqttControlStrategy));
        }
    }
}