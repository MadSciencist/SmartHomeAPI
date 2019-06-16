using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_control_strategy_linkage_type")]
    public class ControlStrategyLinkageType : EntityBase
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public ICollection<ControlStrategyLinkage> Strategies { get; set; }
    }
}
