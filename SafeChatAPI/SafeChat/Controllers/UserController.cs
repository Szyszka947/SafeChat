using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SafeChatAPI.Models;
using SafeChatAPI.Services.Cookies;
using SafeChatAPI.Services.Groups;
using SafeChatAPI.Services.JWT;
using SafeChatAPI.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SafeChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RegisterUserService _registerUserService;
        private readonly LogInUserService _logInUserService;
        private readonly FindUserByEmailService _findUserByEmailService;
        private readonly GenerateRefreshTokenService _generateRefreshTokenService;
        private readonly GetUserInfoFromAccessTokenService _getUserInfoFromAccessTokenService;
        private readonly GetGroupsWhereUserIsMemberOfService _getGroupsWhereUserIsMemberOf;
        private readonly GetUsersWhoNameStartsWithService _getUsersWhoNameStartsWithService;

        public UserController(RegisterUserService registerUserService, LogInUserService logInUserService, FindUserByEmailService findUserByEmailService,
            GenerateRefreshTokenService generateRefreshTokenService, GetUserInfoFromAccessTokenService getUserInfoFromAccessTokenService,
            GetGroupsWhereUserIsMemberOfService getGroupsWhereUserIsMemberOf, GetUsersWhoNameStartsWithService getUsersWhoNameStartsWithService)
        {
            _registerUserService = registerUserService;
            _logInUserService = logInUserService;
            _findUserByEmailService = findUserByEmailService;
            _generateRefreshTokenService = generateRefreshTokenService;
            _getUserInfoFromAccessTokenService = getUserInfoFromAccessTokenService;
            _getGroupsWhereUserIsMemberOf = getGroupsWhereUserIsMemberOf;
            _getUsersWhoNameStartsWithService = getUsersWhoNameStartsWithService;
        }

        // POST api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> OnPostRegister([FromBody] UserRegisterDto userRegisterDto)
        {
            return Ok(await _registerUserService.RegisterAsync(userRegisterDto));
        }

        //POST api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> OnPostLogIn([FromBody] UserLogInDto userLogInDto)
        {
            var logInResult = await _logInUserService.LogInAsync(userLogInDto);

            if (!logInResult.Succeed)
            {
                return StatusCode(StatusCodes.Status400BadRequest, logInResult.ApiResponse);
            }
            else
            {
                HttpContext.Response.Cookies.Append("accessToken", (logInResult.ApiResponse.Result as dynamic).AccessToken, new CookieOptions
                {
                    Path = "/",
                    MaxAge = TimeSpan.FromMinutes(5),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true
                });

                var refreshToken = _generateRefreshTokenService.Generate(userLogInDto.Email);

                if (refreshToken != default)
                {
                    HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
                    {
                        Path = "/api/auth/refresh_token",
                        Expires = refreshToken.Expires,
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        IsEssential = true
                    });
                }

                return StatusCode(StatusCodes.Status200OK, logInResult.ApiResponse);
            }
        }

        //POST api/user/logout
        [HttpPost("logout")]
        [Authorize]
        public void OnPostLogOut()
        {
            HttpContext.Response.Cookies.Delete("accessToken");
            HttpContext.Response.Cookies.Append("refreshToken", "logged out", new CookieOptions
            {
                Path = "/api/auth/refresh_token",
                MaxAge = TimeSpan.FromMilliseconds(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            });//because refresh token cookie path dont equals
        }

        //GET api/user/groups
        [HttpGet("groups")]
        [Authorize]
        public IActionResult GetUserGroups()
        {
            var userInfo = _getUserInfoFromAccessTokenService.GetUserInfo(HttpContext);

            return Ok(new ApiResponse
            {
                Result = _getGroupsWhereUserIsMemberOf.Get(userInfo.Id).Select(p => new GroupInfoDto
                {
                    Id = p.Id,
                    GroupName = p.GroupName,
                    IsGroupPublic = p.IsGroupPublic
                })
            });
        }

        //GET api/user/get?startsWith=
        [HttpGet("get")]
        [Authorize]
        public IActionResult GetUsersWhoNameStartsWith([FromQuery] string startsWith)
        {
            return Ok(_getUsersWhoNameStartsWithService.Get(startsWith).Select(p => new { p.Id, p.UserName }));
        }
    }
}