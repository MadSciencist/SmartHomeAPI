using System;

namespace SmartHome.Core.Infrastructure.Exceptions
{
    public class SmartHomeNodeOfflineException : SmartHomeException
    {
        public SmartHomeNodeOfflineException(string message) : base(message)
        {
        }

        public SmartHomeNodeOfflineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
