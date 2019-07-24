using System.Collections.Generic;

namespace SmartHome.Core.Domain.ContractParams
{
    public class MultipleRelayParam
    {
        public ICollection<SingleRelayParam> Relays { get; set; }
    }
}
