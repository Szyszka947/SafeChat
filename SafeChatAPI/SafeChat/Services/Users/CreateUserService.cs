using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using SafeChatAPI.Models;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Users
{
    public class CreateUserService
    {
        private readonly AppDbContext dbContext;

        public CreateUserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(UserDto userDto)
        {
            dbContext.Users.Add(new UserEntity
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.Password)
            });
            dbContext.SaveChanges();
        }

        public async Task CreateAsync(UserDto userDto)
        {
            await dbContext.Users.AddAsync(new UserEntity
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.Password)
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
