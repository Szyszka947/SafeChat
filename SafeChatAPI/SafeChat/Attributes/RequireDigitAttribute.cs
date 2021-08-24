using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SafeChatAPI.Attributes
{
    public class RequireDigitAttribute : ValidationAttribute
    {
        public RequireDigitAttribute(int digits)
        {
            Digits = digits;
        }

        private int Digits { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value.ToString().Count(char.IsDigit) >= Digits)
                return ValidationResult.Success;

            return new ValidationResult(string.Empty);
        }
    }
}
