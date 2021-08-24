using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using SafeChatAPI.Services.Users;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace SafeChatAPI.Services.JWT
{
    public class GenerateRefreshTokenService
    {
        private readonly FindUserByEmailService _findUserByEmailService;
        private readonly AppDbContext _dbContext;
        private readonly ValidateRefreshTokenService _validateRefreshTokenService;

        public GenerateRefreshTokenService(FindUserByEmailService findUserByEmailService, AppDbContext dbContext, ValidateRefreshTokenService validateRefreshTokenService)
        {
            _findUserByEmailService = findUserByEmailService;
            _dbContext = dbContext;
            _validateRefreshTokenService = validateRefreshTokenService;
        }

        public RefreshToken Generate(string email)
        {
            var user = _findUserByEmailService.FindByEmail(email);

            if (user == default)
                return new RefreshToken();

            var refreshTokens = _dbContext.RefreshTokens.Where(p => p.User == user).Select(p => p).ToList();

            foreach (var item in refreshTokens)
            {
                if (_validateRefreshTokenService.Validate(item.Token) != default)
                {
                    return item;
                }
            }

            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            var refreshToken = new RefreshToken
            {
                User = user,
                Expires = DateTime.UtcNow.AddDays(200),
                Token = Convert.ToBase64String(randomBytes)
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            _dbContext.SaveChanges();

            return refreshToken;
        }
    }
}
