using SmartHome.Core.Entities.Converters;
using SmartHome.Core.MessageHanding;
using System;
using System.Collections.Generic;

namespace SmartHome.Contracts.TasmotaMqtt
{
    public class MappingProfile : NodeDataMapperBase
    {
        protected override void InitializeMapping()
        {
            base.Mapping = new Dictionary<string, string>
            {
                { "POWER", "relay0" },
                { "light", "light" },
                { "Voltage", "voltage" },
                { "Current", "current" },
                { "Power", "power_active" },
                { "ReactivePower", "power_reactive" },
                { "ApparentPower", "power_apparent" },
                { "Factor", "power_factor" },
            };
        }

        protected override void InitializeConverters()
        {
            base.Converters = new Dictionary<string, Type>
            {
                {"relay0", typeof(OnOffToBinaryConverter)}
            };
        }
    }
}
