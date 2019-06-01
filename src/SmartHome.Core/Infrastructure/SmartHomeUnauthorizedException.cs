using System;

namespace SmartHome.Core.Infrastructure
{
    public class SmartHomeUnauthorizedException : UnauthorizedAccessException
    {
        public SmartHomeUnauthorizedException()
        {
        }

        public SmartHomeUnauthorizedException(string message) : base(message)
        {
        }

        public SmartHomeUnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
