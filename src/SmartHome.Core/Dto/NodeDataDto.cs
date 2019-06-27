namespace SmartHome.Core.Dto
{
    public class NodeDataMagnitudeDto
    {
        public string Magnitude { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }

        public NodeDataMagnitudeDto(string magnitude, string value, string unit)
        {
            Magnitude = magnitude;
            Value = value;
            Unit = unit;
        }

        public NodeDataMagnitudeDto()
        {
        }
    }
}
