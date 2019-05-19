﻿using System.Runtime.Serialization;

namespace SmartHome.Core.Infrastructure
{
    public enum MessageType
    {
        [EnumMember(Value = "success")]
        Success = 0,

        [EnumMember(Value = "warning")]
        Warning = 5,

        [EnumMember(Value = "error")]
        Error = 10,

        [EnumMember(Value = "exception")]
        Exception = 15
    }
}