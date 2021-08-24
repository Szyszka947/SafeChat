using Microsoft.EntityFrameworkCore;
using SafeChatAPI.Data.AppDbContext;
using SafeChatAPI.Models;
using SafeChatAPI.Services.Users;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Messages
{
    public class ValidateMessageService
    {
        private readonly AppDbContext _dbContext;
        private readonly FindUserByIdService _findUserByIdService;

        public ValidateMessageService(AppDbContext dbContext, FindUserByIdService findUserByIdService)
        {
            _dbContext = dbContext;
            _findUserByIdService = findUserByIdService;
        }

        public async Task<bool> Validate(SaveMessageDto messageDto, int senderId)
        {
            var user = _findUserByIdService.Find(senderId);

            var isUserGroupMember = (await _dbContext.Groups.Where(p => p.Id == messageDto.GroupId).SelectMany(p => p.Members).ToListAsync()).Contains(user);

            if (!isUserGroupMember) return false;

            return true;
        }
    }
}
