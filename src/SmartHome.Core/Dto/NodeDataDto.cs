using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Dto
{
    public class NodeDataDto
    {
        public PhysicalProperty PhysicalProperty { get; set; }
        public string Value { get; set; }

        public NodeDataDto(PhysicalProperty physicalProperty, string value)
        {
            PhysicalProperty = physicalProperty;
            Value = value;
        }

        public NodeDataDto()
        {
        }
    }
}
