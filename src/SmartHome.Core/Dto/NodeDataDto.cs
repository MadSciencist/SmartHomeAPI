namespace SmartHome.Core.Dto
{
    public class NodeDataDto
    {
        public PhysicalPropertyDto PhysicalProperty { get; set; }
        public string Value { get; set; }

        public NodeDataDto(PhysicalPropertyDto physicalProperty, string value)
        {
            PhysicalProperty = physicalProperty;
            Value = value;
        }

        public NodeDataDto()
        {
        }
    }
}
