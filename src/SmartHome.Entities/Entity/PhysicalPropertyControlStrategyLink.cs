using Matty.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_physicalproperty_controlstrategy_link")]
    public class PhysicalPropertyControlStrategyLink : EntityBase<int>
    {
        public int PhysicalPropertyId { get; set; }
        public int ControlStrategyId { get; set; }
        public PhysicalProperty PhysicalProperty { get; set; }
        public ControlStrategy ControlStrategy { get; set; }
    }
}
