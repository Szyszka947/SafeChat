using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SafeChatAPI.Data;
using SafeChatAPI.Services.Cookies;
using SafeChatAPI.Services.JWT;
using SafeChatAPI.Services.Users;
using System;

namespace SafeChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GetUserInfoFromAccessTokenService _getUserInfoFromAccessTokenService;
        private readonly ValidateRefreshTokenService _validateRefreshTokenService;
        private readonly CookieExistsService _cookieExistsInHeaderService;
        private readonly GetCookieService _getCookieFromHeaderService;
        private readonly GenerateAccessTokenService _generateAccessTokenService;
        private readonly FindUserByRefreshTokenService _findUserByRefreshTokenService;

        public AuthController(GetUserInfoFromAccessTokenService getUserInfoFromAccessTokenService,
            ValidateRefreshTokenService validateRefreshTokenService, GetCookieService getCookieFromHeaderService,
            GenerateAccessTokenService generateAccessTokenService, CookieExistsService cookieExistsInHeaderService,
            FindUserByRefreshTokenService findUserByRefreshTokenService)
        {
            _getUserInfoFromAccessTokenService = getUserInfoFromAccessTokenService;
            _validateRefreshTokenService = validateRefreshTokenService;
            _getCookieFromHeaderService = getCookieFromHeaderService;
            _generateAccessTokenService = generateAccessTokenService;
            _cookieExistsInHeaderService = cookieExistsInHeaderService;
            _findUserByRefreshTokenService = findUserByRefreshTokenService;
        }

        //GET api/auth/user/authenticated
        [HttpGet("user/authenticated")]
        public bool IsUserAuthenticated()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

        //GET api/auth/refresh_token
        [HttpGet("refresh_token")]
        public bool GetAccessTokenByRefreshToken() 
        {
            var tokenExists = _cookieExistsInHeaderService.Exists("refreshToken", HttpContext);

            if (!tokenExists)
                return false;

            var token = _getCookieFromHeaderService.Get("refreshToken", HttpContext);

            if (string.IsNullOrEmpty(token)) return false;

            var refreshToken = _validateRefreshTokenService.Validate(token);

            if (refreshToken)
            {
                var user = _findUserByRefreshTokenService.Find(token);

                if (user == default) return false;

                HttpContext.Response.Cookies.Append("accessToken", _generateAccessTokenService.Generate(user), new CookieOptions
                {
                    Domain = "localhost",
                    Path = "/",
                    MaxAge = TimeSpan.FromMinutes(5),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        //GET api/auth/user_info
        [HttpGet("user_info")]
        [Authorize]
        public string GetUserInfo()
        {
            return JsonConvert.SerializeObject(_getUserInfoFromAccessTokenService.GetUserInfo(HttpContext));
        }
    }
}
