using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace SafeChatAPI.Services.Messages
{
    public class GetGroupImagesService
    {
        private readonly AppDbContext _dbContext;

        public GetGroupImagesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ImageEntity> Get(int groupId)
        {
            return _dbContext.Messages.Where(p => p.Group.Id == groupId).SelectMany(p => p.Images).ToList();
        }
    }
}
