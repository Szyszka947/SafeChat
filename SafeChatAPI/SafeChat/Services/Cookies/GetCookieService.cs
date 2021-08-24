using Microsoft.AspNetCore.Http;

namespace SafeChatAPI.Services.Cookies
{
    public class GetCookieService
    {
        public string Get(string cookieName, HttpContext httpContext)
        {
            return httpContext.Request.Cookies[cookieName];
        }
    }
}
