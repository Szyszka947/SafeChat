using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class CreateGroupDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chat name is too short")]
        [MinLength(3, ErrorMessage = "Chat name is too short")]
        [MaxLength(50, ErrorMessage = "Chat name is too long")]
        public string GroupName { get; set; }
        public bool IsGroupPublic { get; set; }
    }
}
