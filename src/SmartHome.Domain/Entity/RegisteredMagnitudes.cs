using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_registered_magnitude")]
    public class RegisteredMagnitude : EntityBase
    {
        [Required, MaxLength(250)]
        public string Magnitude { get; set; }

        public ControlStrategy ControlStrategy { get; set; }
        public int ControlStrategyId { get; set; }
    }
}
