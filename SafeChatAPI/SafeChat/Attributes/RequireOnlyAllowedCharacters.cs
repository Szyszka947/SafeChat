using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SafeChatAPI.Attributes
{
    public class RequireOnlyAllowedCharacters : ValidationAttribute
    {
        public RequireOnlyAllowedCharacters(string allowedCharacters)
        {
            AllowedCharacters = allowedCharacters;
        }

        private string AllowedCharacters { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            foreach (var item in value.ToString().ToCharArray())
            {
                if (!AllowedCharacters.ToCharArray().Contains(item))
                    return new ValidationResult(string.Empty);
            }

            return ValidationResult.Success;
        }
    }
}
