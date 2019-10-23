using System.Runtime.Serialization;

namespace SmartHome.Contracts.TasmotaMqtt.Domain.Models
{
    internal class EnergyStatusModel
    {
        [DataMember]
        public string Voltage { get; set; }
        [DataMember]
        public string Current { get; set; }
        [DataMember]
        public string Factor { get; set; }
        [DataMember]
        public string Power { get; set; }
        [DataMember]
        public string ApparentPower { get; set; }
        [DataMember]
        public string ReactivePower { get; set; }
    }
}
