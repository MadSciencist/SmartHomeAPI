using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.Domain.DictionaryEntity
{
    [Table("dictionary_value")]
    public class DictionaryValue : EntityBase
    {
        public string Value { get; set; }
        public bool IsActive { get; set; }

        public int DictionaryId { get; set; }
        public Dictionary Dictionary { get; set; }
    }
}