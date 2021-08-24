using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using SafeChatAPI.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SafeChatAPI.Services.JWT
{
    public class GenerateAccessTokenService
    {
        public string Generate(UserEntity user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTConfig.SecretKey));

            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: JWTConfig.ValidIssuer,
                audience: JWTConfig.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials,
                claims: new List<Claim>
                {
                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                    new Claim(JwtClaimTypes.NickName, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                });


            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.WriteToken(tokenOptions);

            return token;
        }
    }
}
