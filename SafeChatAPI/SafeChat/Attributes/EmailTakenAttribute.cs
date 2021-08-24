using SafeChatAPI.Services.Users;
using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Attributes
{
    public class EmailTakenAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var _getIsEmailTakenService = (GetIsEmailTakenService)validationContext.GetService(typeof(GetIsEmailTakenService));

            if (_getIsEmailTakenService.IsEmailTaken(value.ToString()))
                return new ValidationResult(string.Empty);

            return ValidationResult.Success;
        }
    }
}
