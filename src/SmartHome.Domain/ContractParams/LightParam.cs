using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Domain.ContractParams
{
    public class LightParam
    {
        [Range(0, 100)]
        public int Brightness { get; set; }

        [Range(0, 100)]
        public int LightTemperature { get; set; }
    }
}
