using System.Collections.Generic;

namespace SmartHome.Core.Entities.ContractParams
{
    public class MultipleRelayParam
    {
        public ICollection<SingleRelayParam> Relays { get; set; }
    }
}
