using System;

namespace SmartHome.Core.Infrastructure.Exceptions
{
    public class SmartHomeInvalidOperationException : SmartHomeException
    {
        public SmartHomeInvalidOperationException(string message) : base(message)
        {
        }

        public SmartHomeInvalidOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
