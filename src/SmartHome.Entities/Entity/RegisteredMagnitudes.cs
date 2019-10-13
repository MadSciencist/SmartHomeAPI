using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Matty.Framework;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_registered_magnitude")]
    public class RegisteredMagnitude : EntityBase<int>
    {
        [Required, MaxLength(250)]
        public string Magnitude { get; set; }

        public ControlStrategy ControlStrategy { get; set; }
        public int ControlStrategyId { get; set; }
    }
}
