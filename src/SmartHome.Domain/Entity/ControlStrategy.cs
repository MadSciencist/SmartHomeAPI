using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("control_strategy")]
    public class ControlStrategy : EntityBase
    {
        [Required, MaxLength(100)]
        public string Strategy { get; set; }

        [Required, MaxLength(50)]
        public string Key { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
