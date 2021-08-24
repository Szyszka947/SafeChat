using System.Collections.Generic;

namespace SafeChatAPI.Data
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<GroupEntity> GroupsMember { get; set; }
    }
}
