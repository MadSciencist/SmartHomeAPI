using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Domain.ContractParams
{
    public class RgbLightParam
    {
        [Range(0, 2)]
        public int? State { get; set; }

        [Range(0, 255)]
        public byte[] Rgb { get; set; }
    }
}
