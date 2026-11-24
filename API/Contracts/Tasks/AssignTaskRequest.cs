using API.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Tasks
{
    public class AssignTaskRequest
    {
        [Required]
        [NotEmptyGuid]
        public Guid? MemberId { get; set; }
    }
}