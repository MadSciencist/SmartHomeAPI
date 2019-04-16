using SmartHome.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("appuser_node")]
    public class AppUserNode : EntityBase
    {
        public int NodeId { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public Node Node { get; set; }
    }
}
