namespace SmartHome.Contracts.TasmotaMqtt.Domain.Models
{
    internal class EnergyStatusModel
    {
        public string Voltage { get; set; }
        public string Current { get; set; }
        public string Factor { get; set; }
        public string Power { get; set; }
        public string ApparentPower { get; set; }
        public string ReactivePower { get; set; }
    }
}
