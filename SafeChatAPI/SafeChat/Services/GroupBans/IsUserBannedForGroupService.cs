using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System;
using System.Linq;

namespace SafeChatAPI.Services.GroupBans
{
    public class IsUserBannedForGroupService
    {
        private readonly AppDbContext _dbContext;

        public IsUserBannedForGroupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Check(UserEntity user, GroupEntity group)
        {
            var ban = _dbContext.GroupBans.SingleOrDefault(p => p.BannedUser == user && p.BannedForGroup == group);
            if (ban == default) return false;

            return ban.ExpiresAt >= DateTime.UtcNow;
        }
    }
}