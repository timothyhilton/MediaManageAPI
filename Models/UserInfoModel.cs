using System.ComponentModel.DataAnnotations;

namespace MediaManageAPI.Models
{
    public class UserInfoModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
    }
}