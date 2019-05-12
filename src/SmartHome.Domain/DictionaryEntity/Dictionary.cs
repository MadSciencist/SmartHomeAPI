using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.Domain.DictionaryEntity
{
    [Table("dictionary")]
    public class Dictionary : EntityBase
    { 
        public string Name { get; set; }

        public ICollection<DictionaryValue> Values { get; set; }
    }
}
