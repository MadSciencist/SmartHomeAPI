using System.ComponentModel.DataAnnotations;

namespace SmartHome.API.Dto
{
    public class CreateCommandDto
    {
        [Required, MaxLength(50)]
        public string CommandName { get; set; }
        [Required, MaxLength(50)]
        public string ExecutorClassName { get; set; }
    }
}
