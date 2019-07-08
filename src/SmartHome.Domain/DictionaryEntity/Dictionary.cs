using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.Domain.DictionaryEntity
{
    [Table("tbl_dictionary")]
    public class Dictionary : EntityBase
    { 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Metadata { get; set; }
        public bool ReadOnly { get; set; }
        public ICollection<DictionaryValue> Values { get; set; }
    }
}
