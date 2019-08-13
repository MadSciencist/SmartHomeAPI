using SmartHome.Core.MessageHanding;
using System.Collections.Generic;

namespace SmartHome.Contracts.EspurnaMqtt
{
    public class EspurnaMapper : NodeDataMapperBase
    {
        protected override void InitializeMapping()
        {
            base.Mapping = new Dictionary<string, string>
            {
                { "analog", "generic_analog" },
                { "binary", "generic_digital" },
                { "temperature", "temperature" },
                { "relay/0", "relay0" },
                { "relay/1", "relay1" },
                { "relay/2", "relay2" },
                { "relay/3", "relay3" },
                { "humidity", "humidity" },
                { "pressure", "pressure" },
                { "current", "current" },
                { "voltage", "voltage" },
                { "power_active", "power_active" },
                { "power_apparent", "power_apparent" },
                { "power_reactive", "power_reactive" },
                { "energy", "energy" },
                { "energy_delta", "energy_delta" },
                { "generic_event", "generic_event" },
                { "pm1dot0", "pm1dot0" },
                { "pm1dot5", "pm1dot5" },
                { "pm10", "pm10" },
                { "co2", "co2" },
                { "lux", "lux" },
                { "distance", "distance" },
                { "ldr_cpm", "ldr_cpm" },
                { "ldr_uSvh", "ldr_uSvh" },
                { "count", "count" }
            };
        }
    }
}
