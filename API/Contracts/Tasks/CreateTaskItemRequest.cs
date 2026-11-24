using API.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Tasks
{
    public class CreateTaskItemRequest
    {
        [NotEmptyGuid]
        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        public DateOnly? DueDate { get; set; }
    }
}