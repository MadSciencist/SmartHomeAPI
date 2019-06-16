using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_control_strategy_linkage")]
    public class ControlStrategyLinkage : EntityBase
    {
        public string DisplayValue { get; set; }
        public string InternalValue { get; set; }

        // Navigation properties
        public int ControlStrategyId { get; set; }
        public ControlStrategy ControlStrategy { get; set; }
        public int ControlStrategyLinkageTypeId { get; set; }
        public ControlStrategyLinkageType ControlStrategyLinkageType { get; set; }
    }
}
