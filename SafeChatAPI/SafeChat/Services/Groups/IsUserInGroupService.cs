using SafeChatAPI.Services.Users;

namespace SafeChatAPI.Services.Groups
{
    public class IsUserInGroupService
    {
        private readonly FindUserByIdService _findUserByIdService;
        private readonly FindGroupByIdService _findGroupById;
        private readonly GetGroupMembersService _getGroupMembersService;

        public IsUserInGroupService(FindUserByIdService findUserByIdService, FindGroupByIdService findGroupById,
            GetGroupMembersService getGroupMembersService)
        {
            _findUserByIdService = findUserByIdService;
            _findGroupById = findGroupById;
            _getGroupMembersService = getGroupMembersService;
        }

        public bool Check(int userId, int groupId)
        {
            var user = _findUserByIdService.Find(userId);

            var group = _findGroupById.Find(groupId);

            if (group == default || user == default) return false;

            var groupMembers = _getGroupMembersService.Get(groupId);

            if (groupMembers.Contains(user)) return true;

            return false;
        }
    }
}