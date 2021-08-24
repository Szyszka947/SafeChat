using SafeChatAPI.Services.Users;
using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Attributes
{
    public class UserNameTakenAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var _getIsUserNameTakenService = (GetIsUserNameTakenService)validationContext.GetService(typeof(GetIsUserNameTakenService));

            if (_getIsUserNameTakenService.IsUserNameTaken(value.ToString()))
                return new ValidationResult(string.Empty);

            return ValidationResult.Success;
        }
    }
}
