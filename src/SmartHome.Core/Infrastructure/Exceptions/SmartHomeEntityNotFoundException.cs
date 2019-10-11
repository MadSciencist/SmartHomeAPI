using System;

namespace SmartHome.Core.Infrastructure.Exceptions
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
