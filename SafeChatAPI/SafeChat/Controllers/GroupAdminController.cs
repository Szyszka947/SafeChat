using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SafeChatAPI.Data;
using SafeChatAPI.Hubs;
using SafeChatAPI.Models;
using SafeChatAPI.Services.GroupAdmins;
using SafeChatAPI.Services.GroupBans;
using SafeChatAPI.Services.GroupInvitations;
using SafeChatAPI.Services.Groups;
using SafeChatAPI.Services.JWT;
using SafeChatAPI.Services.Users;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SafeChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupAdminController : ControllerBase
    {
        private readonly IsUserInGroupService _isUserInGroupService;
        private readonly IsUserAdminInGroupService _isUserAdminInGroupService;
        private readonly FindUserByIdService _findUserByIdService;
        private readonly GetUserInfoFromAccessTokenService _getUserInfoFromAccessToken;
        private readonly FindGroupByIdService _findGroupByIdService;
        private readonly LeaveGroupService _leaveGroupService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly BanUserService _banUserService;

        public GroupAdminController(IsUserInGroupService isUserInGroupService, IsUserAdminInGroupService isUserAdminInGroupService, FindUserByIdService findUserByIdService,
            GetUserInfoFromAccessTokenService getUserInfoFromAccessToken, FindGroupByIdService findGroupByIdService, LeaveGroupService leaveGroupService,
            IHubContext<ChatHub> hubContext, BanUserService banUserService)
        {
            _isUserInGroupService = isUserInGroupService;
            _isUserAdminInGroupService = isUserAdminInGroupService;
            _findUserByIdService = findUserByIdService;
            _getUserInfoFromAccessToken = getUserInfoFromAccessToken;
            _findGroupByIdService = findGroupByIdService;
            _leaveGroupService = leaveGroupService;
            _hubContext = hubContext;
            _banUserService = banUserService;
        }

        private (IActionResult actionResult, bool allSucceeded) ValidationForKickAndBanGroupMember(UserEntity punishingUser, UserEntity userToPunish, GroupEntity group)
        {
            if (group == default) return (NotFound(new ApiResponse { Result = "Chat not found" }), false);

            if (userToPunish == default)
                return (NotFound(new ApiResponse { Result = "Selected user not found" }), false);

            var isUserToPunishGroupMember = _isUserInGroupService.Check(userToPunish.Id, group.Id);
            if (!isUserToPunishGroupMember) return (NotFound(new ApiResponse { Result = "Selected user are not a member of this chat" }), false);

            var isPunishingUserGroupAdmin = _isUserAdminInGroupService.IsAdmin(punishingUser, group);
            if (!isPunishingUserGroupAdmin) return (BadRequest(new ApiResponse { Result = "You must be the chat administrator" }), false);

            var isUserToPunishGroupAdmin = _isUserAdminInGroupService.IsAdmin(userToPunish, group);
            if (isUserToPunishGroupAdmin) return (BadRequest(new ApiResponse { Result = "You cannot ban or kick the chat administrator" }), false);

            return (Ok(), true);

        }

        //DELETE api/groupadmin/member/kick
        [HttpDelete("member/kick")]
        [Authorize]
        public async Task<IActionResult> KickFromGroupAsync([FromBody] GroupMemberKickDto groupMemberKickDto)
        {
            var kickingUser = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);
            var userToKick = _findUserByIdService.Find(groupMemberKickDto.UserToKickId.Value);
            var group = _findGroupByIdService.Find(groupMemberKickDto.GroupId.Value);

            var (actionResult, allSucceeded) = ValidationForKickAndBanGroupMember(kickingUser, userToKick, group);

            if (!allSucceeded)
                return actionResult;

            _leaveGroupService.Leave(userToKick, group);

            await _hubContext.Clients.User(userToKick.Id.ToString()).SendAsync("ReceivedMemberToKick", group.Id, "You have been kicked from " + group.GroupName +
                " chat by " + kickingUser.UserName + "\nReason: " + groupMemberKickDto.Reason);

            return Ok(new ApiResponse
            {
                Result = userToKick.UserName + " has been kicked from this chat"
            });
        }

        //DELETE api/groupadmin/member/ban
        [HttpDelete("member/ban")]
        [Authorize]
        public async Task<IActionResult> BanGroupMemberAsync([FromBody] GroupMemberBanDto groupMemberBanDto)
        {
            var banningUser = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);
            var userToBan = _findUserByIdService.Find(groupMemberBanDto.UserToBanId.Value);
            var group = _findGroupByIdService.Find(groupMemberBanDto.GroupId.Value);

            var (actionResult, allSucceeded) = ValidationForKickAndBanGroupMember(banningUser, userToBan, group);

            if (!allSucceeded)
                return actionResult;

            var banExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(groupMemberBanDto.BanForDays.Value);

            _banUserService.Ban(new GroupBanEntity
            {
                BannedForGroup = group,
                BannedUser = userToBan,
                Reason = groupMemberBanDto.Reason,
                ExpiresAt = banExpiresAt
            });

            await _hubContext.Clients.User(userToBan.Id.ToString()).SendAsync("ReceivedMemberToBan", group.Id, "You have been banned from the  " + group.GroupName +
                " chat by " + banningUser.UserName + " to " + banExpiresAt.ToString("MMM d, yyyy, h:mm:ss tt", CultureInfo.GetCultureInfo("en-GB")) + "\nReason: " + groupMemberBanDto.Reason);

            return Ok(new ApiResponse
            {
                Result = userToBan.UserName + " was banned for " + groupMemberBanDto.BanForDays + " days for " + groupMemberBanDto.Reason
            });
        }
    }
}
