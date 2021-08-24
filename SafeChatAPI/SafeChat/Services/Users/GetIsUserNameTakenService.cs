using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class GetIsUserNameTakenService
    {
        private readonly FindUserByUserNameService _findUserByUserNameService;

        public GetIsUserNameTakenService(FindUserByUserNameService findUserByUserNameService)
        {
            _findUserByUserNameService = findUserByUserNameService;
        }

        public bool IsUserNameTaken(string userName)
        {
            return _findUserByUserNameService.FindByUserName(userName) != default;
        }

        public async Task<bool> IsUserNameTakenAsync(string userName)
        {
            return await _findUserByUserNameService.FindByUserNameAsync(userName) != default;
        }
    }
}
