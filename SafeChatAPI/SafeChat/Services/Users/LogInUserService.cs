using Microsoft.AspNetCore.Identity;
using SafeChatAPI.Models;
using SafeChatAPI.Services.JWT;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class LogInUserService
    {
        private readonly FindUserByEmailService _findUserByEmailService;
        private readonly GenerateAccessTokenService _generateAccessTokenService;

        public LogInUserService(FindUserByEmailService findUserByEmailService, GenerateAccessTokenService generateAccessTokenService)
        {
            _findUserByEmailService = findUserByEmailService;
            _generateAccessTokenService = generateAccessTokenService;
        }

        public async Task<(ApiResponse ApiResponse, bool Succeed)> LogInAsync(UserLogInDto userLogInDto)
        {
            var user = await _findUserByEmailService.FindByEmailAsync(userLogInDto.Email);

            if (user == default)
            {
                return (new ApiResponse
                {
                    Result = new
                    {
                        Email = "No user found with this e-mail address"
                    }
                }, false);
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(userLogInDto.Password, user.PasswordHash))
                return (new ApiResponse
                {
                    Result = new
                    {
                        AccessToken = _generateAccessTokenService.Generate(user)
                    }
                }, true);
            else
                return (new ApiResponse
                {
                    Result = new 
                    {
                        Password = "Wrong password"
                    }
                }, false);
        }
    }
}
