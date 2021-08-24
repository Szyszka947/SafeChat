using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SafeChatAPI.Data;
using SafeChatAPI.Data.AppDbContext;
using SafeChatAPI.Hubs;
using SafeChatAPI.Models;
using SafeChatAPI.Services.Files;
using SafeChatAPI.Services.Groups;
using SafeChatAPI.Services.JWT;
using SafeChatAPI.Services.Messages;
using SafeChatAPI.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly GetUserInfoFromAccessTokenService _getUserInfoFromAccessToken;
        private readonly ValidateMessageService _validateMessageService;
        private readonly SaveFileService _saveFileService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly SaveMessageService _saveMessageService;
        private readonly FindGroupByIdService _findGroupByIdService;


        public MessageController(GetUserInfoFromAccessTokenService getUserInfoFromAccessToken, ValidateMessageService validateMessageService,
            SaveFileService saveFileService, IHubContext<ChatHub> hubContext, SaveMessageService saveMessageService,
            FindGroupByIdService findGroupByIdService)
        {
            _getUserInfoFromAccessToken = getUserInfoFromAccessToken;
            _validateMessageService = validateMessageService;
            _saveFileService = saveFileService;
            _hubContext = hubContext;
            _saveMessageService = saveMessageService;
            _findGroupByIdService = findGroupByIdService;
        }

        //POST api/chat/message/send
        [HttpPost("send")]
        [Authorize]
        [RequestSizeLimit(30_000_000)] // max 30mb
        public async Task<IActionResult> SendMessage([FromForm] SaveMessageDto saveMessageDto)
        {
            var contentSizeInKb = Encoding.UTF8.GetByteCount(saveMessageDto.Content) / 1000;

            if (contentSizeInKb > 15) // content max 32kb
            {
                return BadRequest(new ApiResponse() { Result = "Message is too long" });
            }

            var sender = _getUserInfoFromAccessToken.GetUserInfo(HttpContext);

            var response = await _validateMessageService.Validate(saveMessageDto, sender.Id);

            if (response == true)
            {
                var imageUrls = saveMessageDto.FormFiles != default ? await _saveFileService.Save(saveMessageDto.FormFiles, Origins.SafeChatAPI) : new List<string>();

                var messageType = saveMessageDto.FormFiles != default && saveMessageDto.Content == default ? MessageTypes.Image :
                    saveMessageDto.FormFiles == default && saveMessageDto.Content != default ? MessageTypes.Text : MessageTypes.Mixed;


                await _hubContext.Clients.Group(saveMessageDto.GroupId.ToString()).SendAsync(
                    "ReceivedMessage",
                    saveMessageDto.GroupId,
                    saveMessageDto.Content,
                    imageUrls,
                    saveMessageDto.DateTime,
                    sender.UserName,
                    messageType);

                await _saveMessageService.SaveAsync(new MessageEntity
                {
                    Content = saveMessageDto.Content,
                    Images = imageUrls.Select(p => new ImageEntity { ImageUrl = p }).ToList(),
                    DateTime = saveMessageDto.DateTime,
                    Group = _findGroupByIdService.Find(saveMessageDto.GroupId.Value),
                    SenderId = sender.Id,
                    Type = messageType
                });

                return Ok(new ApiResponse { Result = "Message sended" });
            }
            return BadRequest(new ApiResponse { Result = "User is not member of this group" });
        }
    }
}
