namespace SmartHome.Core.Entities.Entity
{
    public class PhysicalProperty
    {
        public string Description { get; set; }
        public string Magnitude { get; set; }
        public string Unit { get; set; }

        #region ctors
        public PhysicalProperty()
        {
        }

        public PhysicalProperty(string description, string magnitude, string unit)
        {
            Description = description;
            Magnitude = magnitude;
            Unit = unit;
        }

        public PhysicalProperty(string magnitude, string unit)
        {
            Magnitude = magnitude;
            Unit = unit;
        }
        #endregion
    }
}
