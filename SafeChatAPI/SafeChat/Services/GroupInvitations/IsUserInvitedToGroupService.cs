using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.GroupInvitations
{
    public class IsUserInvitedToGroupService
    {
        private readonly AppDbContext _dbContext;

        public IsUserInvitedToGroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Check(UserEntity user, GroupEntity group)
        {
            return _dbContext.GroupInvitations.SingleOrDefault(p => p.InvitedUser == user && p.Group == group) != default;
        }
    }
}
