using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("controllable_node")]
    public class ControllableNode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string BasicAuthLogin { get; set; }

        [MaxLength(50)]
        public string BasicAuhPassword { get; set; }

        [MaxLength(250)]
        public string RegisteredProperties { get; set; }

        [MaxLength(20)]
        public string IpAddress { get; set; }

        [MaxLength(20)]
        public string GatewayIpAddress { get; set; }

        public bool IsOn { get; set; }
    }
}
