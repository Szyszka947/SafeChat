using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class GroupIdDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select chat ID")]
        public int? GroupId { get; set; }
    }
}
