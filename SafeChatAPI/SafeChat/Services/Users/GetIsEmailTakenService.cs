using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class GetIsEmailTakenService
    {
        private readonly FindUserByEmailService _findUserByEmailService;

        public GetIsEmailTakenService(FindUserByEmailService findUserByEmailService)
        {
            _findUserByEmailService = findUserByEmailService;
        }

        public bool IsEmailTaken(string email)
        {
            return _findUserByEmailService.FindByEmail(email) != default;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _findUserByEmailService.FindByEmailAsync(email) != default;
        }
    }
}
