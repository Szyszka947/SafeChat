using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SafeChatAPI.Attributes
{
    public class RequireLowerCaseAttribute : ValidationAttribute
    {
        public RequireLowerCaseAttribute(int lowerCases)
        {
            LowerCases = lowerCases;
        }

        private int LowerCases { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value.ToString().Count(char.IsLower) >= LowerCases)
                return ValidationResult.Success;

            return new ValidationResult(string.Empty);
        }
    }
}
