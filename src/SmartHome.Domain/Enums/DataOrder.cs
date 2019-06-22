using System.Runtime.Serialization;

namespace SmartHome.Core.Domain.Enums
{
    public enum DataOrder
    {
        [EnumMember(Value = "ASC")]
        Asc = 1,
        [EnumMember(Value = "DESC")]
        Desc = 2
    }
}
