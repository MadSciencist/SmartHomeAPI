using SmartHome.Core.MessageHanding;
using System.Collections.Generic;

namespace SmartHome.Contracts.TasmotaMqtt
{
    public class MappingProfile : NodeDataMapperBase
    {
        protected override void InitializeMapping()
        {
            base.Mapping = new Dictionary<string, string>
            {
                { "POWER", "relay0" }
            };
        }
    }
}
