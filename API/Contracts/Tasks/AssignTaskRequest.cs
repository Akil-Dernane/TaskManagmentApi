using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Tasks
{
    public class AssignTaskRequest
    {
        [Required]
        public Guid? MemberId { get; set; }
    }
}
