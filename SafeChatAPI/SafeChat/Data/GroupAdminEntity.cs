namespace SafeChatAPI.Data
{
    public class GroupAdminEntity
    {
        public int Id { get; set; }
        public UserEntity Admin { get; set; }
        public GroupEntity Group { get; set; }
    }
}
