using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Entities.ContractParams
{
    public class LightParam : ValidatableParamBase<LightParam>
    {
        /// <summary>
        /// Set the device on (1), off(0) or toggles (2).
        /// </summary>
        [Range(0, 2)]
        [Required]
        public int? State { get; set; }

        [Range(0, 100)]
        public int? Brightness { get; set; }

        [Range(0, 100)]
        public int? LightTemperature { get; set; }
    }
}
