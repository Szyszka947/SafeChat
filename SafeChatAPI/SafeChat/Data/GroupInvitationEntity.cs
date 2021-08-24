namespace SafeChatAPI.Data
{
    public class GroupInvitationEntity
    {
        public int Id { get; set; }
        public GroupEntity Group { get; set; }
        public UserEntity InvitedUser { get; set; }
    }
}
