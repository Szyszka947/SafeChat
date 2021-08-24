using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace SafeChatAPI.Services.Messages
{
    public class GetGroupMessagesService
    {
        private readonly AppDbContext _dbContext;

        public GetGroupMessagesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MessageEntity> Get(int groupId)
        {
            return _dbContext.Messages.Where(p => p.Group.Id == groupId).Select(p => p).ToList();
        }
    }
}
