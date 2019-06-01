using System.ComponentModel.DataAnnotations;

namespace SmartHome.API.Dto
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(255)]
        public string Login { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }
    }
}