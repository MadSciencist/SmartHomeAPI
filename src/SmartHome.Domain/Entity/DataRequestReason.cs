using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Domain.Entity
{
    [Table("tbl_data_request_reason")]
    public class DataRequestReason : EntityBase
    {
        [MaxLength(50)]
        public string Reason { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        // Navigation property
        public ICollection<NodeData> NodeData { get; set; }
    }
}
