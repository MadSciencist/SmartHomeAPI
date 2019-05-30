using System;

namespace SmartHome.Core.Infrastructure
{
    public class SmartHomeEntityNotFoundException : Exception
    {
        public SmartHomeEntityNotFoundException(string message) : base(message)
        {
        }

        public SmartHomeEntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
