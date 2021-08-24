using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.GroupAdmins
{
    public class RemoveGroupAdminService
    {
        private readonly AppDbContext _dbContext;

        public RemoveGroupAdminService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Remove(UserEntity user, GroupEntity group)
        {
            var groupAdmin = _dbContext.GroupsAdmins.SingleOrDefault(p => p.Admin == user && p.Group == group);
            _dbContext.GroupsAdmins.Remove(groupAdmin);
        }
    }
}
