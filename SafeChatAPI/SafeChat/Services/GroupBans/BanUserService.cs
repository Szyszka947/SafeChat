using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using SafeChatAPI.Services.Groups;

namespace SafeChatAPI.Services.GroupBans
{
    public class BanUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly LeaveGroupService _leaveGroupService;

        public BanUserService(AppDbContext dbContext, LeaveGroupService leaveGroupService)
        {
            _dbContext = dbContext;
            _leaveGroupService = leaveGroupService;
        }

        public void Ban(GroupBanEntity groupBanEntity)
        {
            _dbContext.GroupBans.Add(new GroupBanEntity
            {
                BannedUser = groupBanEntity.BannedUser,
                BannedForGroup = groupBanEntity.BannedForGroup,
                Reason = groupBanEntity.Reason,
                ExpiresAt = groupBanEntity.ExpiresAt
            
            });

            _leaveGroupService.Leave(groupBanEntity.BannedUser, groupBanEntity.BannedForGroup);
            _dbContext.SaveChanges();
        }
    }
}
