using System.Runtime.Serialization;

namespace SmartHome.Core.Entities.Enums
{
    public enum UiConfigurationType
    {
        [EnumMember(Value = "dashboard")]
        Dashboard = 1,

        [EnumMember(Value = "dashboardMembers")]
        DashboardMembers = 2,

        [EnumMember(Value = "control")]
        Control = 10,

        [EnumMember(Value = "controlMembers")]
        ControlMembers = 11
    }
}
