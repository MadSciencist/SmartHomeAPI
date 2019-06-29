using SmartHome.Core.Domain.Models;
using System.Collections.Generic;
using System.Linq;

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

        public static PhysicalProperty GetPhysicalPropertyByContextDictionary(IDictionary<string, string> contextDictionary, string magnitude)
        {
            if (contextDictionary.TryGetValue(magnitude, out var value))
            {
                return GetPhysicalProperty(value);
            }

            return null;
        }

        public static PhysicalProperty GetPhysicalProperty(string magnitude)
        {
            return Properties.SingleOrDefault(x => x.Magnitude == magnitude);
        }

        private static void InitValues()
        {
            Properties = new List<PhysicalProperty>
            {
                new PhysicalProperty("temperature", "C"),
                new PhysicalProperty("humidity", "%"),
                new PhysicalProperty("pressure", "hPa"),
                new PhysicalProperty("current", "A"),
                new PhysicalProperty("voltage", "V"),
                new PhysicalProperty("power_active", "W"),
                new PhysicalProperty("power_apparent", "VA"),
                new PhysicalProperty("power_reactive", "var"),
                new PhysicalProperty("power_factor", "%"),
                new PhysicalProperty("energy", "kWh"),
                new PhysicalProperty("energy_delta", "kWh"),
                new PhysicalProperty("generic_analog", "bit"),
                new PhysicalProperty("generic_digital", "binary"),
                new PhysicalProperty("generic_event", ""),
                new PhysicalProperty("relay0", "bit"),
                new PhysicalProperty("relay1", "bit"),
                new PhysicalProperty("relay2", "bit"),
                new PhysicalProperty("relay3", "bit"),
                //{"", new PhysicalValue("pm1dot0", "ppm")},
                //{"", new PhysicalValue("pm1dot5", "ppm")},
                //{"", new PhysicalValue("pm10", "ppm")},
                //{"", new PhysicalValue("co2", "ppm")},
                //{"", new PhysicalValue("lux", "lux")},
                //{"", new PhysicalValue("distance", "m")},
                //{"", new PhysicalValue("ldr_cpm", "events")},
                //{"", new PhysicalValue("ldr_uSvh", "mSv")},
                //{"", new PhysicalValue("count", "events")},
            };
        }
    }
}
