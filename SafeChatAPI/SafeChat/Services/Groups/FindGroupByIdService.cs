using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.Groups
{
    public class FindGroupByIdService
    {
        private readonly AppDbContext _dbContext;

        public FindGroupByIdService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GroupEntity Find(int groupId)
        {
            return _dbContext.Groups.SingleOrDefault(p => p.Id == groupId);
        }
    }
}
