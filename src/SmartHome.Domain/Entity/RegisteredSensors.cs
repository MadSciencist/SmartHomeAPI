using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_registered_sensors")]
    public class RegisteredSensors : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
