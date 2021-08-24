using Microsoft.AspNetCore.Http;

namespace SafeChatAPI.Services.Cookies
{
    public class CookieExistsService
    {
        public bool Exists(string cookieName, HttpContext httpContext)
        {
            return httpContext.Request.Cookies.TryGetValue(cookieName, out string _);
        }
    }
}
