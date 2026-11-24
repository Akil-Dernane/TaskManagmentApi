using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Members
{
    public class UpdateMemberRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}