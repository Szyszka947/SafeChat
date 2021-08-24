using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class SaveMessageDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select group ID")]
        public int? GroupId { get; set; }
        public DateTime DateTime { get; } = DateTime.UtcNow;
        public string Content { get; set; }
        public List<IFormFile> FormFiles { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Message cannot be empty")]
        public bool IsMessageValid => Content != default || FormFiles != default;
    }
}
