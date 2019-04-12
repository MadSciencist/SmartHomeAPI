using System;
using SmartHome.Domain.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.Entity
{
    [Table("node")]
    public class Node
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Required]
        public string Identifier { get; set; }

        public NodeType Type { get; set; }

        public ControllableNode ControllableNode { get; set; }

        public int CreatedById { get; set; }
        public virtual AppUser CreatedBy { get; set; }

        public DateTime Created { get; set; }
    }
}
