using System.ComponentModel.DataAnnotations;
using SmartHome.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("appuser_node")]
    public class AppUserNode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NodeId { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public Node Node { get; set; }
    }
}
