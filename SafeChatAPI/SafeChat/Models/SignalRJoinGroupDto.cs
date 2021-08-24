using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class SignalRJoinGroupDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Connection id is required")]
        public string ConnectionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Group id is required")]
        public int? GroupId { get; set; }
    }
}
