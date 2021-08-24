using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class FindUserByIdService
    {
        private readonly AppDbContext _dbContext;

        public FindUserByIdService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserEntity Find(int id)
        {
            return _dbContext.Users.SingleOrDefault(p => p.Id == id);
        }

        public async Task<UserEntity> FindAsync(int id)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(p => p.Id == id);
        }

    }
}
