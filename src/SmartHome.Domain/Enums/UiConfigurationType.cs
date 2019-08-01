using System.Runtime.Serialization;

namespace SmartHome.Core.Domain.Enums
{
    public enum UiConfigurationType
    {
        [EnumMember(Value = "dashboard")]
        Dashboard = 1,

        [EnumMember(Value = "control")]
        Control = 10
    }
}
