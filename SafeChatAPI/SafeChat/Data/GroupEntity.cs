using System.Collections.Generic;

namespace SafeChatAPI.Data
{
    public class GroupEntity
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public ICollection<UserEntity> Members { get; set; }
        public bool IsGroupPublic { get; set; }
    }
}
