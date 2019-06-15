using System.ComponentModel.DataAnnotations.Schema;
using SmartHome.Core.Domain.Entity;

namespace SmartHome.Core.Domain.DictionaryEntity
{
    [Table("tbl_dictionary_value")]
    public class DictionaryValue : EntityBase
    {
        public string DisplayValue { get; set; }
        public string InternalValue { get; set; }
        public bool IsActive { get; set; }

        public int DictionaryId { get; set; }
        public Dictionary Dictionary { get; set; }
    }
}