using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;

namespace SafeChatAPI.Services.Groups
{
    public class CreateGroupService
    {
        private readonly AppDbContext _dbContext;

        public CreateGroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(GroupEntity group)
        {
            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();
        }
    }
}
