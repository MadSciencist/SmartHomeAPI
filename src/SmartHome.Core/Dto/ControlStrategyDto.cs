namespace SmartHome.Core.Dto
{
    public class ControlStrategyDto
    {
        public string Description { get; set; }
        public string ControlContext { get; set; }
        public string ReceiveContext { get; set; }
        public string ReceiveProvider { get; set; }
        public string ControlProvider { get; set; }
        public bool IsActive { get; set; }
    }
}
