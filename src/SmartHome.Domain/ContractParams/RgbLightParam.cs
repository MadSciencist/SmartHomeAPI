using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Domain.ContractParams
{
    public class RgbLightParam : LightParam
    {
        [Range(0, 255)]
        public byte R { get; set; }

        [Range(0, 255)]
        public byte G { get; set; }

        [Range(0, 255)]
        public byte B { get; set; }
    }
}
