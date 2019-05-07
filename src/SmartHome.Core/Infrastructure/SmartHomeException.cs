using System;

namespace SmartHome.Core.Infrastructure
{
    public class SmartHomeException : Exception
    {
        public SmartHomeException(string message) : base(message)
        {
        }

        public SmartHomeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
