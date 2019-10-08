using SmartHome.Core.Entities.Models;

namespace SmartHome.Core.Dto
{
    public class NodeDataMagnitudeDto
    {
        public PhysicalProperty PhysicalProperty { get; set; }
        public string Value { get; set; }

        public NodeDataMagnitudeDto(PhysicalProperty physicalProperty, string value)
        {
            PhysicalProperty = physicalProperty;
            Value = value;
        }

        public NodeDataMagnitudeDto()
        {
        }
    }
}
