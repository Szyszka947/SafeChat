using Microsoft.IdentityModel.Tokens;
using SafeChatAPI.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SafeChatAPI.Services.JWT
{
    public class ValidateAccessTokenService
    {
        public bool Validate(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(accessToken)) return false;
            var claimsPrincipal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidAudience = JWTConfig.ValidAudience,
                ValidIssuer = JWTConfig.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTConfig.SecretKey)),
                ClockSkew = TimeSpan.FromMinutes(1)
            }, out _);

            return claimsPrincipal.Identity.IsAuthenticated;
        }
    }
}
