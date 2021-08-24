using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.Users
{
    public class FindUserByRefreshTokenService
    {
        private readonly AppDbContext _dbContext;

        public FindUserByRefreshTokenService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserEntity Find(string token)
        {
            return _dbContext.RefreshTokens.Where(p => p.Token.Equals(token)).Select(p => p.User).FirstOrDefault();
        }
    }
}
