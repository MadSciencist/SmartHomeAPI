using Matty.Framework;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;
using Matty.Framework.Abstractions;

namespace SmartHome.Core.Entities.Entity
{
    [Table("tbl_ui_configuration")]
    public class UiConfiguration : EntityBase<int>
    {
        public UiConfigurationType Type { get; set; }
        public string Data { get; set; }

        // Navigation property
        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
