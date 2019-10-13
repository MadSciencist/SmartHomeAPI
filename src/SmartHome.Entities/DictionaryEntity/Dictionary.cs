using Matty.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.DictionaryEntity
{
    [Table("tbl_dictionary")]
    public class Dictionary : EntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Metadata { get; set; }
        public bool ReadOnly { get; set; }
        public ICollection<DictionaryValue> Values { get; set; }
    }
}
