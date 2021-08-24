using SafeChatAPI.Data.AppDbContext;
using System;

namespace SafeChatAPI.Data
{
    public class GroupBanEntity
    {
        public int Id { get; set; }
        public UserEntity BannedUser { get; set; }
        public GroupEntity BannedForGroup { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Reason { get; set; }
    }
}
