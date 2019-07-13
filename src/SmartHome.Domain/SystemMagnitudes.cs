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

        public static PhysicalProperty GetPhysicalPropertyByContractMapping(IDictionary<string, string> contractDict, string magnitude)
        {
            if (contractDict.TryGetValue(magnitude, out var value))
            {
                return GetPhysicalPropertyByMagnitude(value);
            }

            return null;
        }

        public static PhysicalProperty GetPhysicalPropertyByMagnitude(string magnitude)
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
