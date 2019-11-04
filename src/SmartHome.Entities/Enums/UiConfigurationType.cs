using System.Runtime.Serialization;

namespace SmartHome.Core.Entities.Enums
{
    public enum UiConfigurationType
    {
        /// <summary>
        /// Config of whole page
        /// </summary>
        [EnumMember(Value = "page")]
        Page = 1,

        /// <summary>
        /// Page member config
        /// </summary>
        [EnumMember(Value = "member")]
        Member = 2
    }
}
