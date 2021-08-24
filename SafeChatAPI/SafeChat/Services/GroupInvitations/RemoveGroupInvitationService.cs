using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.GroupInvitations
{
    public class RemoveGroupInvitationService
    {
        private readonly AppDbContext _dbContext;

        public RemoveGroupInvitationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Remove(UserEntity user, GroupEntity group)
        {
            var groupInvitation = _dbContext.GroupInvitations.SingleOrDefault(p => p.Group == group && p.InvitedUser == user);
            _dbContext.GroupInvitations.Remove(groupInvitation);
            _dbContext.SaveChanges();
        }
    }
}
