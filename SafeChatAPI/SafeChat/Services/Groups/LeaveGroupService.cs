using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;

namespace SafeChatAPI.Services.Groups
{
    public class LeaveGroupService
    {
        private readonly AppDbContext _dbContext;

        public LeaveGroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Leave(UserEntity user, GroupEntity group)
        {
            _dbContext.Entry(group).Collection("Members").Load();
            _dbContext.Entry(user).Collection("GroupsMember").Load();

            group.Members.Remove(user);
            user.GroupsMember.Remove(group);

            _dbContext.SaveChanges();
        }
    }
}
