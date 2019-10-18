using System.Runtime.Serialization;

namespace Matty.Framework.Enums
{
    public enum DataOrder
    {
        [EnumMember(Value = "ASC")]
        Asc = 1,

        [EnumMember(Value = "DESC")]
        Desc = 2
    }
}
