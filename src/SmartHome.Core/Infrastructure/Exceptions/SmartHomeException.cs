using System;

namespace SmartHome.Core.Infrastructure.Exceptions
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
