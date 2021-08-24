using SafeChatAPI.Data.AppDbContext;
using System;
using System.Collections.Generic;

namespace SafeChatAPI.Data
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public GroupEntity Group { get; set; }
        public int SenderId { get; set; }
        public MessageTypes Type { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public ICollection<ImageEntity> Images { get; set; }
    }
}
