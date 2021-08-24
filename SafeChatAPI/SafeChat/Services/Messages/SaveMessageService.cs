using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Messages
{
    public class SaveMessageService
    {
        private readonly AppDbContext _dbContext;

        public SaveMessageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(MessageEntity messageEntity)
        {
            await _dbContext.Messages.AddAsync(messageEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
