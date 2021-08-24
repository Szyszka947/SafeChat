using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.Groups
{
    public class IsPublicGroupNameTakenService
    {
        private readonly AppDbContext _dbContext;

        public IsPublicGroupNameTakenService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsTaken(string groupName)
        {
            return _dbContext.Groups.SingleOrDefault(p => p.GroupName.Equals(groupName) && p.IsGroupPublic == true) != default;
        }
    }
}
