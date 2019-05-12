using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Domain.User;

namespace SmartHome.Core.Domain.Entity
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
