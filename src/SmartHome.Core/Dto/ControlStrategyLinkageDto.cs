namespace SmartHome.Core.Dto
{
    public class ControlStrategyLinkageDto
    {
        public string DisplayValue { get; set; }
        public string InternalValue { get; set; }

        public ControlStrategyLinkageDto(string displayValue, string internalValue)
        {
            DisplayValue = displayValue;
            InternalValue = internalValue;
        }

        public ControlStrategyLinkageDto()
        { 
        }
    }
}
