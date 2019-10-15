using System.Runtime.Serialization;

namespace SmartHome.Core.Entities.Enums
{
    public enum JobStatus
    {
        [EnumMember(Value = "Running")]
        Running = 1,

        [EnumMember(Value = "Paused")]
        Paused = 10
    }
}
