using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Tasks
{
    public class ChangeTaskPriorityRequest
    {
        [Required]
        [EnumDataType(typeof(ProjectTaskPriority))]
        public ProjectTaskPriority? Priority { get; set; }
    }
}