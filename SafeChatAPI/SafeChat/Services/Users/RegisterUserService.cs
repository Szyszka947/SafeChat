using SafeChatAPI.Models;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class RegisterUserService
    {
        private readonly CreateUserService _createUserService;

        public RegisterUserService(CreateUserService createUserService)
        {
            _createUserService = createUserService;
        }

        public async Task<ApiResponse> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            await _createUserService.CreateAsync(new UserDto
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                Password = userRegisterDto.Password
            });

            return new ApiResponse
            {
                Result = "User created successfully"
            };
        }
    }
}
