using System.ComponentModel.DataAnnotations;

namespace SmartHome.API.DTO
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
