using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SafeChatAPI.Hubs;
using SafeChatAPI.Models;
using SafeChatAPI.Services.Groups;
using SafeChatAPI.Services.JWT;
using SafeChatAPI.Services.Messages;
using SafeChatAPI.Services.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SafeChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRGroupController : ControllerBase
    {
        private readonly GetUserInfoFromAccessTokenService _getUserInfoFromAccessToken;
        private readonly IsUserInGroupService _isUserInGroupService;
        private readonly GetGroupsWhereUserIsMemberOfService _getGroupsWhereUserIsMemberOfService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly GetGroupMessagesService _getGroupMessages;
        private readonly GetImagesByMessageIdService _getImagesByMessageIdService;
        private readonly FindUserByIdService _findUserByIdService;


        public SignalRGroupController(GetUserInfoFromAccessTokenService getUserInfoFromAccessToken, IsUserInGroupService isUserInGroupService,
            GetGroupsWhereUserIsMemberOfService getGroupsWhereUserIsMemberOfService, IHubContext<ChatHub> hubContext, GetGroupMessagesService getGroupMessages,
            GetImagesByMessageIdService getImagesByMessageIdService, FindUserByIdService findUserByIdService)
        {
            _getUserInfoFromAccessToken = getUserInfoFromAccessToken;
            _isUserInGroupService = isUserInGroupService;
            _getGroupsWhereUserIsMemberOfService = getGroupsWhereUserIsMemberOfService;
            _hubContext = hubContext;
            _getGroupMessages = getGroupMessages;
            _getImagesByMessageIdService = getImagesByMessageIdService;
            _findUserByIdService = findUserByIdService;
        }

        //POST api/signalrgroup/join
        [HttpPost("join")] // it's only for activate group real-time chat and show group messages for user who successfully requested this
        [Authorize]
        public async Task<IActionResult> JoinGroup([FromBody] SignalRJoinGroupDto joinGroupDto)
        {
            var userInfo = _getUserInfoFromAccessToken.GetUserInfo(HttpContext);

            var isUserInGroup = _isUserInGroupService.Check(userInfo.Id, joinGroupDto.GroupId.Value);

            if (!isUserInGroup)
                return BadRequest(new ApiResponse
                {
                    Result = "You are not a member of this group"
                });

            var userGroups = await _getGroupsWhereUserIsMemberOfService.GetAsync(userInfo.Id);

            foreach (var item in userGroups)
            {
                await _hubContext.Groups.RemoveFromGroupAsync(joinGroupDto.ConnectionId, item.Id.ToString());
            }

            await _hubContext.Groups.AddToGroupAsync(joinGroupDto.ConnectionId, joinGroupDto.GroupId.Value.ToString());

            var groupMessages = _getGroupMessages.Get(joinGroupDto.GroupId.Value);

            groupMessages.Reverse();

            return Ok(groupMessages.Select(p => new ShowMessageDto
            {
                GroupId = p.Group.Id,
                DateTime = p.DateTime,
                Content = p.Content,
                ImageUrls = _getImagesByMessageIdService.Get(p.Id).Select(p => p.ImageUrl).ToList(),
                MessageType = p.Type,
                SenderName = _findUserByIdService.Find(p.SenderId).UserName
            }));
        }
    }
}
