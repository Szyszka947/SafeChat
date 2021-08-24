using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;

namespace SafeChatAPI.Services.GroupAdmins
{
    public class AddGroupAdminService
    {
        private readonly AppDbContext _dbContext;

        public AddGroupAdminService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(GroupAdminEntity groupAdmin)
        {
            _dbContext.GroupsAdmins.Add(groupAdmin);
            _dbContext.SaveChanges();
        }
    }
}
