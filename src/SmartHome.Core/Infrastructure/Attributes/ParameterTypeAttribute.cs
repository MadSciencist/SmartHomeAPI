using System;

namespace SmartHome.Core.Infrastructure.Attributes
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
