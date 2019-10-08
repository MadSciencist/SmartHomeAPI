using SmartHome.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.DictionaryEntity
{
    [Table("tbl_dictionary_value")]
    public class DictionaryValue : EntityBase
    {
        public string DisplayValue { get; set; }
        public string InternalValue { get; set; }
        public bool? IsActive { get; set; }
        public string Metadata { get; set; }

        public int DictionaryId { get; set; }
        public Dictionary Dictionary { get; set; }
    }
}