namespace SmartHome.Core.Domain.Models
{
    public class PhysicalProperty
    {
        public string Magnitude { get; set; }
        public string Unit { get; set; }

        public PhysicalProperty()
        {
        }

        public PhysicalProperty(string magnitude, string unit)
        {
            Magnitude = magnitude;
            Unit = unit;
        }
    }
}
