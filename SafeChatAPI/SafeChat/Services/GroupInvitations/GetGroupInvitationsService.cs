using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.GroupInvitations
{
    public class GetGroupInvitationsService
    {
        private readonly AppDbContext _dbContext;

        public GetGroupInvitationsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<GroupInvitationEntity>> Get(UserEntity user)
        {
            return await _dbContext.GroupInvitations.Where(p => p.InvitedUser == user).Include(p => p.Group).Select(p => p).ToListAsync();
        }
    }
}
