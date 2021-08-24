using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Groups
{
    public class GetGroupsWhereUserIsMemberOfService
    {
        private readonly AppDbContext _dbContext;

        public GetGroupsWhereUserIsMemberOfService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<GroupEntity> Get(int userId)
        {
            return _dbContext.Users.Where(p => p.Id == userId).SelectMany(p => p.GroupsMember).ToList();
        }

        public async Task<List<GroupEntity>> GetAsync(int userId)
        {
            return await _dbContext.Users.Where(p => p.Id == userId).SelectMany(p => p.GroupsMember).ToListAsync();
        }
    }
}
