using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Entities.ContractParams
{
    public class SingleRelayParam
    {
        [Range(0, 255)]
        public string RelayNo { get; set; }

        [Range(0, 2)]
        public string State { get; set; }
    }
}
