using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.GroupAdmins
{
    public class IsUserAdminInGroupService
    {
        private readonly AppDbContext _dbContext;

        public IsUserAdminInGroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsAdmin(UserEntity user, GroupEntity group)
        {
            return _dbContext.GroupsAdmins.SingleOrDefault(p => p.Admin == user && p.Group == group) != default;
        }
    }
}
