using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GetUserInfoFromAccessTokenService _getUserInfoFromAccessToken;
        private readonly FindUserByIdService _findUserByIdService;
        private readonly IsPublicGroupNameTakenService _isPublicGroupNameTakenService;
        private readonly CreateGroupService _createGroupService;
        private readonly AddGroupAdminService _addGroupAdminService;
        private readonly FindGroupByIdService _findGroupByIdService;
        private readonly IsUserInGroupService _isUserInGroupService;
        private readonly IsUserBannedForGroupService _isUserBannedForGroupService;
        private readonly GetGroupBanService _getGroupBanService;
        private readonly JoinGroupService _joinGroupService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly GetAllPublicGroupsService _getAllPublicGroupsService;
        private readonly IsUserAdminInGroupService _isUserAdminInGroupService;
        private readonly RemoveGroupAdminService _removeGroupAdminService;
        private readonly LeaveGroupService _leaveGroupService;
        private readonly IsUserInvitedToGroupService _isUserInvitedToGroupService;
        private readonly AddGroupInviteService _addGroupInviteService;
        private readonly GetGroupMembersService _getGroupMembersService;

        public GroupController(GetUserInfoFromAccessTokenService getUserInfoFromAccessToken, FindUserByIdService findUserByIdService,
            IsPublicGroupNameTakenService isPublicGroupNameTakenService, CreateGroupService createGroupService, AddGroupAdminService addGroupAdminService,
            FindGroupByIdService findGroupByIdService, IsUserInGroupService isUserInGroupService, IsUserBannedForGroupService isUserBannedForGroupService,
            GetGroupBanService getGroupBanService, JoinGroupService joinGroupService, IHubContext<ChatHub> hubContext, GetAllPublicGroupsService getAllPublicGroupsService,
            IsUserAdminInGroupService isUserAdminInGroupService, RemoveGroupAdminService removeGroupAdminService, LeaveGroupService leaveGroupService,
            IsUserInvitedToGroupService isUserInvitedToGroupService, AddGroupInviteService addGroupInviteService, GetGroupMembersService getGroupMembersService)
        {
            _getUserInfoFromAccessToken = getUserInfoFromAccessToken;
            _findUserByIdService = findUserByIdService;
            _isPublicGroupNameTakenService = isPublicGroupNameTakenService;
            _createGroupService = createGroupService;
            _addGroupAdminService = addGroupAdminService;
            _findGroupByIdService = findGroupByIdService;
            _isUserInGroupService = isUserInGroupService;
            _isUserBannedForGroupService = isUserBannedForGroupService;
            _getGroupBanService = getGroupBanService;
            _joinGroupService = joinGroupService;
            _hubContext = hubContext;
            _getAllPublicGroupsService = getAllPublicGroupsService;
            _isUserAdminInGroupService = isUserAdminInGroupService;
            _removeGroupAdminService = removeGroupAdminService;
            _leaveGroupService = leaveGroupService;
            _isUserInvitedToGroupService = isUserInvitedToGroupService;
            _addGroupInviteService = addGroupInviteService;
            _getGroupMembersService = getGroupMembersService;
        }

        //POST api/group/create
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto createGroupDto)
        {
            var userInfo = _getUserInfoFromAccessToken.GetUserInfo(HttpContext);

            var creator = await _findUserByIdService.FindAsync(userInfo.Id);

            if (createGroupDto.IsGroupPublic)
            {
                var isPublicGroupNameTaken = _isPublicGroupNameTakenService.IsTaken(createGroupDto.GroupName);

                if (isPublicGroupNameTaken) return Conflict(new ApiResponse { Result = "Chat name is taken" });
            }

            var groupEntity = new GroupEntity
            {
                GroupName = createGroupDto.GroupName,
                IsGroupPublic = createGroupDto.IsGroupPublic,
                Members = new List<UserEntity> { creator }
            };

            _createGroupService.Create(groupEntity);
            _addGroupAdminService.Add(new GroupAdminEntity { Admin = creator, Group = groupEntity });

            return StatusCode(StatusCodes.Status201Created, new ApiResponse { Result = "Chat created successfully" });
        }

        //POST api/group/join
        [HttpPost("join")]
        [Authorize]
        public async Task<IActionResult> JoinGroup([FromBody] GroupIdDto joinGroupDto)
        {
            var group = _findGroupByIdService.Find(joinGroupDto.GroupId.Value);
            if (group == default || group.IsGroupPublic == false) return BadRequest(new ApiResponse { Result = "You cannot join this group" });

            var user = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);

            var isUserAlreadyInGroup = _isUserInGroupService.Check(user.Id, group.Id);

            if (isUserAlreadyInGroup) return Conflict(new ApiResponse { Result = "You are already in this group" });

            var isUserBannedInGroup = _isUserBannedForGroupService.Check(user, group);

            if (isUserBannedInGroup)
            {
                var banInfo = _getGroupBanService.Get(user, group);

                var date = banInfo.ExpiresAt.ToString("MMM d, yyyy, h:mm:ss tt", CultureInfo.GetCultureInfo("en-GB"));
                var readyDate = char.ToUpper(date[0]) + date[1..];

                return BadRequest(new ApiResponse
                {
                    Result = "You are banned on this group to " + readyDate + "\nReason: " + banInfo.Reason
                });
            }

            _joinGroupService.Join(user, group);

            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceivedGroupJoin", group.Id, group.GroupName, group.IsGroupPublic);

            return StatusCode(StatusCodes.Status201Created, new ApiResponse { Result = "Joined the " + group.GroupName + " chat" });
        }

        //GET api/group/public/all
        [HttpGet("public/all")]
        [Authorize]
        public async Task<IActionResult> GetPublicGroups() =>
            Ok((await _getAllPublicGroupsService.Get()).Select(p => new GetAllPublicGroupsDto { Id = p.Id, GroupName = p.GroupName }));

        //DELETE api/group/leave
        [HttpDelete("leave")]
        [Authorize]
        public async Task<IActionResult> LeaveGroupAsync([FromBody] GroupIdDto leaveGroupDto)
        {
            var group = _findGroupByIdService.Find(leaveGroupDto.GroupId.Value);

            if (group == default) return BadRequest(new ApiResponse { Result = "Chat not found" });

            var user = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);

            var isUserGroupMember = _isUserInGroupService.Check(user.Id, group.Id);
            if (!isUserGroupMember) return NotFound(new ApiResponse { Result = "You are not a member of this chat" });

            var isUserGroupAdmin = _isUserAdminInGroupService.IsAdmin(user, group);

            if (isUserGroupAdmin) _removeGroupAdminService.Remove(user, group);

            _leaveGroupService.Leave(user, group);

            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceivedGroupLeave", group.Id);

            return StatusCode(StatusCodes.Status201Created, new ApiResponse { Result = "Leaved the " + group.GroupName + " chat" });
        }

        //POST api/group/invite
        [HttpPost("invite")]
        [Authorize]
        public async Task<IActionResult> InviteToGroupAsync([FromBody] GroupInvitationDto groupInvitationDto)
        {
            var invitingUser = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);

            var invitedUser = _findUserByIdService.Find(groupInvitationDto.InvitedUserId.Value);

            if (invitedUser == default)
                return NotFound(new ApiResponse { Result = "Selected user not found" });

            var group = _findGroupByIdService.Find(groupInvitationDto.GroupId.Value);

            if (group == default) return NotFound(new ApiResponse { Result = "Chat not found" });

            if (!group.IsGroupPublic)
            {
                var isInvitingUserGroupAdmin = _isUserAdminInGroupService.IsAdmin(invitingUser, group);
                if (!isInvitingUserGroupAdmin) return BadRequest(new ApiResponse { Result = "You must be the chat administrator" });
            }

            var isInvitedUserBannedForThisGroup = _isUserBannedForGroupService.Check(invitedUser, group);

            if (isInvitedUserBannedForThisGroup) return BadRequest(
                new ApiResponse
                {
                    Result = invitedUser.UserName + " is banned on this chat"
                });

            var isUserAlreadyInGroup = _isUserInGroupService.Check(invitedUser.Id, group.Id);
            if (isUserAlreadyInGroup) return Conflict(new ApiResponse { Result = invitedUser.UserName + " are already in this chat" });

            var isUserAlreadyInvitedToGroup = _isUserInvitedToGroupService.Check(invitedUser, group);
            if (isUserAlreadyInvitedToGroup) return Conflict(new ApiResponse { Result = invitedUser.UserName + " are already invited" });

            _addGroupInviteService.Add(invitedUser, group);
            await _hubContext.Clients.User(invitedUser.Id.ToString()).SendAsync("ReceivedNewInvitation", group.Id, group.GroupName);

            return Ok(new ApiResponse
            {
                Result = invitedUser.UserName + " invited to " + group.GroupName + " chat"
            });
        }

        //GET api/group/members
        [HttpGet("members")]
        [Authorize]
        public IActionResult GetGroupMembers([FromQuery] int groupId)
        {
            return Ok(_getGroupMembersService.Get(groupId).Select(p => new { p.Id, p.UserName }));
        }
    }
}