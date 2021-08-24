using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace SafeChatAPI.Services.Groups
{
    public class JoinGroupService
    {
        private readonly AppDbContext _dbContext;
        private readonly GetGroupMembersService _getGroupMembersService;


        public JoinGroupService(AppDbContext dbContext, GetGroupMembersService getGroupMembersService)
        {
            _dbContext = dbContext;
            _getGroupMembersService = getGroupMembersService;
        }

        public void Join(UserEntity user, GroupEntity group)
        {
            var groupMembers = _getGroupMembersService.Get(group.Id);

            group.Members = new List<UserEntity>();
            group.Members.Add(user);

            _dbContext.SaveChanges();
        }
    }
}
