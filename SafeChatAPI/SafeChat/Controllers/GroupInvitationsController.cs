using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SafeChatAPI.Hubs;
using SafeChatAPI.Models;
using SafeChatAPI.Services.GroupInvitations;
using SafeChatAPI.Services.Groups;
using SafeChatAPI.Services.JWT;
using SafeChatAPI.Services.Users;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupInvitationsController : ControllerBase
    {
        private readonly GetUserInfoFromAccessTokenService _getUserInfoFromAccessToken;
        private readonly FindUserByIdService _findUserByIdService;
        private readonly GetGroupInvitationsService _getGroupInvitationsService;
        private readonly IsUserInvitedToGroupService _isUserInvitedToGroupService;
        private readonly FindGroupByIdService _findGroupByIdService;
        private readonly JoinGroupService _joinGroupService;
        private readonly IsUserInGroupService _isUserInGroupService;
        private readonly RemoveGroupInvitationService _removeGroupInvitationService;
        private readonly IHubContext<ChatHub> _hubContext;

        public GroupInvitationsController(GetGroupInvitationsService getGroupInvitationsService, GetUserInfoFromAccessTokenService getUserInfoFromAccessToken,
            FindUserByIdService findUserByIdService, FindGroupByIdService findGroupByIdService, JoinGroupService joinGroupService, IsUserInGroupService isUserInGroupService,
            RemoveGroupInvitationService removeGroupInvitationService, IsUserInvitedToGroupService isUserInvitedToGroupService, IHubContext<ChatHub> hubContext)
        {
            _getGroupInvitationsService = getGroupInvitationsService;
            _getUserInfoFromAccessToken = getUserInfoFromAccessToken;
            _findUserByIdService = findUserByIdService;
            _findGroupByIdService = findGroupByIdService;
            _joinGroupService = joinGroupService;
            _isUserInGroupService = isUserInGroupService;
            _removeGroupInvitationService = removeGroupInvitationService;
            _isUserInvitedToGroupService = isUserInvitedToGroupService;
            _hubContext = hubContext;
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetInvitations()
        {
            var user = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);
            return Ok((await _getGroupInvitationsService.Get(user)).Select(p => new { InvitationId = p.Id, p.Group.GroupName, GroupId = p.Group.Id }));
        }

        //POST api/groupinvitations/accept
        [HttpPost("accept")]
        [Authorize]
        public async Task<IActionResult> AcceptInvitation([FromBody][Required(AllowEmptyStrings = false, ErrorMessage = "Select group invitation")] int? groupId)
        {
            var user = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);

            var group = _findGroupByIdService.Find(groupId.Value);

            var isUserInvited = _isUserInvitedToGroupService.Check(user, group);

            if (!isUserInvited) return NotFound(new ApiResponse { Result = "You are not invited to this chat" });

            var isUserInGroup = _isUserInGroupService.Check(user.Id, group.Id);

            if (isUserInGroup) return Conflict(new ApiResponse { Result = "You are already in this chat" });

            _joinGroupService.Join(user, group);
            _removeGroupInvitationService.Remove(user, group);
            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceivedInvitationAction", group.Id);
            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceivedGroupJoin", group.Id, group.GroupName, group.IsGroupPublic);

            return Ok(new ApiResponse { Result = "You joined " + group.GroupName + " chat" });
        }

        //DELETE api/groupinvitations/discard
        [HttpDelete("discard")]
        [Authorize]
        public async Task<IActionResult> DiscardInvitationAsync([FromBody][Required(AllowEmptyStrings = false, ErrorMessage = "Select group invitation")] int? groupId)
        {
            var user = _findUserByIdService.Find(_getUserInfoFromAccessToken.GetUserInfo(HttpContext).Id);

            var group = _findGroupByIdService.Find(groupId.Value);

            var isUserInvited = _isUserInvitedToGroupService.Check(user, group);

            if (!isUserInvited) return NotFound(new ApiResponse { Result = "You are not invited to this chat" });

            _removeGroupInvitationService.Remove(user, group);
            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceivedInvitationAction", group.Id);

            return Ok(new ApiResponse { Result = "You have declined a " + group.GroupName + " chat invitation" });
        }
    }
}
