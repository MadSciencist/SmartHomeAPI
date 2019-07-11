using SmartHome.Core.Domain.Enums;
using System;

namespace SmartHome.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControlContractAttribute : Attribute
    {
        public ContractType ContractType { get; private set; }

        public ControlContractAttribute(ContractType contractType)
        {
            ContractType = contractType;
        }
    }
}
