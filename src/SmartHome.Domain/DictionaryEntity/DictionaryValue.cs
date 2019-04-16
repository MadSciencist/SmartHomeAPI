using SmartHome.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Domain.DictionaryEntity
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