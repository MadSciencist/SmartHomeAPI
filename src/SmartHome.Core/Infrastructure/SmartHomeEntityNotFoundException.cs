using System;

namespace SmartHome.Core.Infrastructure
{
    public class SmartHomeEntityNotFoundException : SmartHomeException
    {
        public SmartHomeEntityNotFoundException(string message) : base(message)
        {
        }

        public SmartHomeEntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
