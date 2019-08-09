using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Domain.ContractParams
{
    public class LightParam
    {
        /// <summary>
        /// Set the device on (1), off(0) or toggles (2).
        /// Overrides brightness.
        /// </summary>
        [Range(0, 2)]
        public int? State { get; set; }

        [Range(0, 100)]
        public int? Brightness { get; set; }

        [Range(0, 100)]
        public int? LightTemperature { get; set; }
    }
}
