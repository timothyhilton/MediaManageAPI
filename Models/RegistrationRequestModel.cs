using System.ComponentModel.DataAnnotations;

namespace MediaManageAPI.Models
{
    public class RegistrationRequestModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
