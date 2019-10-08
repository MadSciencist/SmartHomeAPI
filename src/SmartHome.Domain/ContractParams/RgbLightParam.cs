using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Entities.ContractParams
{
    public class RgbLightParam : ValidatableParamBase<RgbLightParam>
    {
        [Range(0, 2)]
        [Required]
        public int? State { get; set; }

        [Range(0, 255)]
        public byte[] Rgb { get; set; }
    }
}
