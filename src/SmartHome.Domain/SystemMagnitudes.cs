using SmartHome.Core.Domain.Models;
using System.Collections.Generic;

namespace SmartHome.Core.Domain
{
    /// <summary>
    /// This class defines all magnitudes that system is able to collect
    /// </summary>
    public static class SystemMagnitudes
    {
        public static ICollection<PhysicalProperty> Properties { get; private set; }

        static SystemMagnitudes()
        {
            InitValues();
        }
        
        private static void InitValues()
        {
            Properties = new List<PhysicalProperty>
            {
                new PhysicalProperty("Temperature", "temperature", "C"),
                new PhysicalProperty("Humidity", "humidity", "%"),
                new PhysicalProperty("Pressure", "pressure", "hPa"),
                new PhysicalProperty("Current", "current", "A"),
                new PhysicalProperty("Voltage", "voltage", "V"),
                new PhysicalProperty("power_active", "W"),
                new PhysicalProperty("power_apparent", "VA"),
                new PhysicalProperty("power_reactive", "var"),
                new PhysicalProperty("power_factor", "%"),
                new PhysicalProperty("energy", "kWh"),
                new PhysicalProperty("energy_delta", "kWh"),
                new PhysicalProperty("Generic Analog Sensor", "generic_analog", "bit"),
                new PhysicalProperty("Generic digital (binary) Sensor", "generic_digital", "binary"),
                new PhysicalProperty("generic_event", ""),
                new PhysicalProperty("relay0", "bit"),
                new PhysicalProperty("relay1", "bit"),
                new PhysicalProperty("relay2", "bit"),
                new PhysicalProperty("relay3", "bit"),
                new PhysicalProperty("pm1dot0", "ppm"),
                new PhysicalProperty("pm1dot5", "ppm"),
                new PhysicalProperty("pm10", "ppm"),
                new PhysicalProperty("co2", "ppm"),
                new PhysicalProperty("lux", "lux"),
                new PhysicalProperty("distance", "m"),
                new PhysicalProperty("ldr_cpm", "events"),
                new PhysicalProperty("ldr_uSvh", "mSv"),
                new PhysicalProperty("count", "events"),
            };
        }
    }
}
