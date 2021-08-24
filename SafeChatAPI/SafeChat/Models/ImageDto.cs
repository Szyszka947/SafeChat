using Microsoft.AspNetCore.Http;

namespace SafeChatAPI.Models
{
    public class ImageDto
    {
        public IFormFile FormFile { get; set; }
    }
}
