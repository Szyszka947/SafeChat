using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Groups
{
    public class GetAllPublicGroupsService
    {
        private readonly AppDbContext _dbContext;

        public GetAllPublicGroupsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<GroupEntity>> Get()
        {
            return await _dbContext.Groups.Where(p => p.IsGroupPublic == true).Select(p => p).ToListAsync();
        }
    }
}
