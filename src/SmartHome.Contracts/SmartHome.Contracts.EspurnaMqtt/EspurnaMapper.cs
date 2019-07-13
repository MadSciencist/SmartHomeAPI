using SmartHome.Core.Domain.Models;
using SmartHome.Core.MessageHanding;
using System.Collections.Generic;

namespace SmartHome.Contracts.EspurnaMqtt
{
    public class EspurnaMapper : NodeDataMapperBase, INodeDataMapper
    {
        public EspurnaMapper()
        {
            InitializeMapping();
            InitializeValidProperties();
        }

        private void InitializeMapping()
        {
            // TODO rest of mappings
            base.Mapping = new Dictionary<string, string>
            {
                {"relay/0", "relay0"},
                {"relay/1", "relay1"},
                {"relay/2", "relay2"},
                {"relay/3", "relay3"},
                {"analog", "generic_analog" }
            };
        }

        private void InitializeValidProperties()
        {
            // https://github.com/xoseperez/espurna/wiki/MQTT
            base.ValidProperties = new List<PhysicalProperty>
            {
                new PhysicalProperty("temperature", "C"),
                new PhysicalProperty("humidity", "%"),
                new PhysicalProperty("pressure", "hPa"),
                new PhysicalProperty("current", "A"),
                new PhysicalProperty("voltage", "V"),
                new PhysicalProperty("power", "W"),
                new PhysicalProperty("apparent", "W"),
                new PhysicalProperty("reactive", "W"),
                new PhysicalProperty("factor", "%"),
                new PhysicalProperty("energy", "kWh"),
                new PhysicalProperty("energy_delta", "kWh"),
                new PhysicalProperty("analog", "bit"),
                new PhysicalProperty("digital", "binary"),
                new PhysicalProperty("event", ""),
                new PhysicalProperty("pm1dot0", "ppm"),
                new PhysicalProperty("pm1dot5", "ppm"),
                new PhysicalProperty("pm10", "ppm"),
                new PhysicalProperty("co2", "ppm"),
                new PhysicalProperty("lux", "lux"),
                new PhysicalProperty("distance", "m"),
                new PhysicalProperty("hcho", "ppm"),
                new PhysicalProperty("ldr_cpm", "events"),
                new PhysicalProperty("ldr_uSvh", "mSv"),
                new PhysicalProperty("count", "events"),
                new PhysicalProperty("relay/0", "bit"),
                new PhysicalProperty("relay/1", "bit"),
                new PhysicalProperty("relay/2", "bit"),
                new PhysicalProperty("relay/3", "bit"),
            };
        }
    }
}
