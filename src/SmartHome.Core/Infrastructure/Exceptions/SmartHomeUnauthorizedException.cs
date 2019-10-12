using System;

namespace SmartHome.Core.Infrastructure.Exceptions
{
    public class SmartHomeUnauthorizedException : SmartHomeException
    {
        public SmartHomeUnauthorizedException(string message) : base(message)
        {
        }

        public SmartHomeUnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
