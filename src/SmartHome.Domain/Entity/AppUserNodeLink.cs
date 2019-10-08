using SmartHome.Core.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_user_node_link")]
    public class AppUserNodeLink : EntityBase
    {
        public int NodeId { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public Node Node { get; set; }
    }
}
