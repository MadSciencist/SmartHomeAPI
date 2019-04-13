using SmartHome.Domain.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.DictionaryEntity
{
    [Table("dictionary")]
    public class Dictionary : EntityBase
    { 
        public string Name { get; set; }

        public ICollection<DictionaryValue> Values { get; set; }
    }
}
