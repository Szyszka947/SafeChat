using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class GroupInvitationDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Group ID is required")]
        public int? GroupId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select user to invite")]
        public int? InvitedUserId { get; set; }
    }
}
