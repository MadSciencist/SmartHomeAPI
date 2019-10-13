using System;

namespace SmartHome.Core.Entities.Attributes
{
    public class ParameterTypeAttribute : Attribute
    {
        public Type ParameterType { get; set; }

        public ParameterTypeAttribute(Type parameterType)
        {
            ParameterType = parameterType;
        }
    }
}
