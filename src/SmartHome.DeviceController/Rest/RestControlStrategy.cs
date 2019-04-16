using System;

namespace SmartHome.DeviceController.Rest
{
    public class RestControlStrategy : IControlStrategy
    {
        public void Execute()
        {
            Console.WriteLine(nameof(RestControlStrategy));
        }
    }
}