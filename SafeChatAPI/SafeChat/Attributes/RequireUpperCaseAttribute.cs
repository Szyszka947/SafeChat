using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SafeChatAPI.Attributes
{
    public class RequireUpperCaseAttribute : ValidationAttribute
    {
        public RequireUpperCaseAttribute(int upperCases)
        {
            UpperCases = upperCases;
        }

        private int UpperCases { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value.ToString().Count(char.IsUpper) >= UpperCases)
                return ValidationResult.Success;

            return new ValidationResult(string.Empty);
        }
    }
}
