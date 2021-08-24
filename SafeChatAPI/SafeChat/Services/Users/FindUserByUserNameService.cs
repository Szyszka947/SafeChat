using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class FindUserByUserNameService
    {
        private readonly AppDbContext dbContext;

        public FindUserByUserNameService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public UserEntity FindByUserName(string userName)
        {
            return dbContext.Users.SingleOrDefault(p => p.UserName.Equals(userName));
        }

        public async Task<UserEntity> FindByUserNameAsync(string userName)
        {
            return await dbContext.Users.SingleOrDefaultAsync(p => p.UserName.Equals(userName));
        }
    }
}
