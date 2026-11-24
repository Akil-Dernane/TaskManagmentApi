using API.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Projects
{
    public class TransferProjectOwnershipRequest
    {
        [Required]
        [NotEmptyGuid]
        public Guid? NewOwnerMemberId { get; set; }
    }
}