using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class GroupMemberKickDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select user to kick")]
        public int? UserToKickId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select group")]
        public int? GroupId { get; set; }
        public string Reason { get; set; }
    }
}
