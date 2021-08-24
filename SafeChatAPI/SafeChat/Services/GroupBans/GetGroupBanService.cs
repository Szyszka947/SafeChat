using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;

namespace SafeChatAPI.Services.GroupBans
{
    public class GetGroupBanService
    {
        private readonly AppDbContext _dbContext;

        public GetGroupBanService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GroupBanEntity Get(UserEntity user, GroupEntity group)
        {
            return _dbContext.GroupBans.SingleOrDefault(p => p.BannedUser == user && p.BannedForGroup == group);
        }
    }
}
