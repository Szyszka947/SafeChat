using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class FindUserByEmailService
    {
        private readonly AppDbContext dbContext;

        public FindUserByEmailService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public UserEntity FindByEmail(string email)
        {
            return dbContext.Users.SingleOrDefault(p => p.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<UserEntity> FindByEmailAsync(string email)
        {
            return await dbContext.Users.SingleOrDefaultAsync(p => p.Email.ToLower().Equals(email.ToLower()));
        }
    }
}
