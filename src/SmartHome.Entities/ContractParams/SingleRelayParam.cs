using Matty.Framework.Validation;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Entities.ContractParams
{
    public class SingleRelayParam : ValidatableParamBase<SingleRelayParam>
    {
        [Range(0, 255)]
        [Required]
        public string RelayNo { get; set; }

        [Range(0, 2)]
        [Required]
        public string State { get; set; }
    }
}
