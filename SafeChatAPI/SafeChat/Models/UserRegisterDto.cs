using SafeChatAPI.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class UserRegisterDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username is too short")]
        [MaxLength(25, ErrorMessage = "Username is too long")]
        [UserNameTaken(ErrorMessage = "Username is taken")]
        [RequireOnlyAllowedCharacters("AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz123456789!@#$%^&*(){}[]", ErrorMessage = "Username contains not allowed characters")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail is required")]
        [EmailAddress(ErrorMessage = "Wrong e-mail")]
        [EmailTaken(ErrorMessage = "E-mail is taken")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password is too short")]
        [RequireDigit(1, ErrorMessage = "Password must contain at least one number")]
        [RequireLowerCase(1, ErrorMessage = "Password must contain at least one lowercase letter")]
        [RequireUpperCase(1, ErrorMessage = "Password must contain at least one uppercase letter")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Repeat password")]
        public string RepeatPassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Passwords do not matches")]
        public bool PasswordsMatches => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(RepeatPassword) ? Password.Equals(RepeatPassword) : false;
    }
}