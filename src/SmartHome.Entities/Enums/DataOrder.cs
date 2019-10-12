﻿using System.Runtime.Serialization;

namespace SmartHome.Core.Entities.Enums
{
    public enum DataOrder
    {
        [EnumMember(Value = "ASC")]
        Asc = 1,
        [EnumMember(Value = "DESC")]
        Desc = 2
    }
}