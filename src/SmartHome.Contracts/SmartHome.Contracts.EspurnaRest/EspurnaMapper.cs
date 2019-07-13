using SmartHome.Core.MessageHanding;
using System.Collections.Generic;

namespace SmartHome.Contracts.EspurnaRest
{
    public class EspurnaMapper : NodeDataMapperBase
    {
        protected override void InitializeMapping()
        {
            // TODO rest of mappings, read from json???
            base.Mapping = new Dictionary<string, string>
            {
                {"relay/0", "relay0"},
                {"relay/1", "relay1"},
                {"relay/2", "relay2"},
                {"relay/3", "relay3"},
                {"analog", "generic_analog" }
            };
        }
    }
}
