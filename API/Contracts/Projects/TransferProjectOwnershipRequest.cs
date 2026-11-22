using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Projects
{
    public class TransferProjectOwnershipRequest
    {
        [Required]
        public Guid? NewOwnerMemberId { get; set; }
    }
}