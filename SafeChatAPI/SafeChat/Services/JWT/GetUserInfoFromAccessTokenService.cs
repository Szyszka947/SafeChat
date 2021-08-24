using Microsoft.AspNetCore.Http;
using SafeChatAPI.Models;
using SafeChatAPI.Services.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SafeChatAPI.Services.JWT
{
    public class GetUserInfoFromAccessTokenService
    {
        private readonly GetCookieService _getCookieService;

        public GetUserInfoFromAccessTokenService(GetCookieService getCookieService)
        {
            _getCookieService = getCookieService;
        }

        public UserInfoDto GetUserInfo(HttpContext httpContext)
        {
            var accessToken = _getCookieService.Get("accessToken", httpContext);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenAsJson = tokenHandler.ReadJwtToken(accessToken);

            var userId = tokenAsJson.Claims.First(claim => claim.Type == "id").Value;
            var userName = tokenAsJson.Claims.First(claim => claim.Type == "nickname").Value;

            return new UserInfoDto
            {
                Id = int.Parse(userId),
                UserName = userName
            };
        }
    }
}
