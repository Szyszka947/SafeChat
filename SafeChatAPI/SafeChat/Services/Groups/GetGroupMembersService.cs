using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SafeChatAPI.Services.Groups
{
    public class GetGroupMembersService
    {
        private readonly AppDbContext _dbContext;

        public GetGroupMembersService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<UserEntity> Get(int groupId)
        {
            return _dbContext.Groups.Where(p => p.Id == groupId).SelectMany(p => p.Members).ToList();
        }
    }
}
