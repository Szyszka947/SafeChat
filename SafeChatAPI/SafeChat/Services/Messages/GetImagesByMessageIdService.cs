using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace SafeChatAPI.Services.Messages
{
    public class GetImagesByMessageIdService
    {
        private readonly AppDbContext _dbContext;

        public GetImagesByMessageIdService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ImageEntity> Get(int messageId)
        {
            return _dbContext.Messages.Where(p => p.Id == messageId).SelectMany(p => p.Images).ToList();
        }
    }
}
