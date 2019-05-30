using System.ComponentModel.DataAnnotations;

namespace SmartHome.Core.Dto
{
    public class CommandEntityDto
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Alias { get; set; }
        [Required, MaxLength(100)]
        public string ExecutorClassName { get; set; }
    }
}
