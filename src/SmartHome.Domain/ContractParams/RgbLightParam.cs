namespace SmartHome.Core.Domain.ContractParams
{
    public class RgbLightParam
    {
        public byte Brightness { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public bool UseFade { get; set; }
    }
}
