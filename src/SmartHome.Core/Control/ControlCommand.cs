namespace SmartHome.Core.Control
{
    public class ControlCommand
    {
        public EControlCommand CommandType { get; set; }
        public object Payload { get; set; }
    }

    public enum EControlCommand
    {
        GetState = 0,
        SetState = 1
    }
}
