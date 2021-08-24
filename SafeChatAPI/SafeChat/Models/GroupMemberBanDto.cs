using System.ComponentModel.DataAnnotations;

namespace SafeChatAPI.Models
{
    public class GroupMemberBanDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select user to ban")]
        public int? UserToBanId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select group")]
        public int? GroupId { get; set; }
        public string Reason { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter the length of the ban (days)")]
        public double? BanForDays { get; set; }
    }
}
