using System;

namespace SmartHome.Contracts.TasmotaMqtt.Domain.Models
{
    internal class EnergyStatusWrapperModel
    {
        public DateTime Time { get; set; }
        public EnergyStatusModel Energy { get; set; }
    }
}
