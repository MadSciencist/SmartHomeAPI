namespace SmartHome.Core.Domain
{
    public class PhysicalValue
    {
        public string Magnitude { get; set; }
        public string Unit { get; set; }

        public PhysicalValue()
        {
        }

        public PhysicalValue(string magnitude, string unit)
        {
            Magnitude = magnitude;
            Unit = unit;
        }
    }
}
