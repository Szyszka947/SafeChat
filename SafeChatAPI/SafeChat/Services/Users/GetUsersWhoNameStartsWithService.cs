using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace SafeChatAPI.Services.Users
{
    public class GetUsersWhoNameStartsWithService
    {
        private readonly AppDbContext _dbContext;

        public GetUsersWhoNameStartsWithService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<UserEntity> Get(string startsWith)
        {
            return _dbContext.Users.Where(p => p.UserName.StartsWith(startsWith)).Select(p => p).ToList();
        }
    }
}
