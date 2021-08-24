using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class UserLogInDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail is required")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
