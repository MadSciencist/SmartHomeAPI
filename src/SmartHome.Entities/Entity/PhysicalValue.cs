using Matty.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_physical_property")]
    public class PhysicalProperty : EntityBase<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplex { get; set; }
        public string Unit { get; set; }
        public string Magnitude { get; set; }
    }
}
