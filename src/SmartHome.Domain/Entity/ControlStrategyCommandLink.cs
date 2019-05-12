using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_strategy_command_link")]
    public class ControlStrategyCommandLink : EntityBase
    {
        public int CommandId { get; set; }
        public int ControlStrategyId { get; set; }
        public ControlStrategy ControlStrategy { get; set; }
        public Command Command { get; set; }
    }
}
