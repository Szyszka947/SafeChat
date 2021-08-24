using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.JWT
{
    public class ValidateRefreshTokenService
    {
        private readonly AppDbContext _dbContext;

        public ValidateRefreshTokenService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Validate(string token)
        {
            var refreshToken = _dbContext.RefreshTokens.SingleOrDefault(p => p.Token.Equals(token));

            if (refreshToken == default)
                return false;

            if (refreshToken.IsActive && !refreshToken.IsExpired)
            {
                return true;
            }

            return false;
        }
    }
}
