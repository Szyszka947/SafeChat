using SafeChatAPI.Data;
using System;
using System.Collections.Generic;

namespace SafeChatAPI.Models
{
    public class ShowMessageDto
    {
        public int GroupId { get; set; }
        public string SenderName { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public List<string> ImageUrls { get; set; }
        public MessageTypes MessageType { get; set; }
    }
}
