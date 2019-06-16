using System.ComponentModel.DataAnnotations;

namespace SmartHome.API.Dto
{
    public class DictionaryEntryDto
    {
        [Required, MinLength(1), MaxLength(50)]
        public string DisplayValue { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string InternalValue { get; set; }
        public bool? IsActive { get; set; }
    }
}
