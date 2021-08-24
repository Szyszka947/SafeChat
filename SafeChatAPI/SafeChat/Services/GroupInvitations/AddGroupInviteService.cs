using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;

namespace SafeChatAPI.Services.GroupInvitations
{
    public class AddGroupInviteService
    {
        private readonly AppDbContext _dbContext;

        public AddGroupInviteService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(UserEntity invitedUser, GroupEntity group)
        {
            _dbContext.GroupInvitations.Add(new GroupInvitationEntity { InvitedUser = invitedUser, Group = group });
            _dbContext.SaveChanges();
        }
    }
}
