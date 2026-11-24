using API.Validation;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Projects
{
    public class AddProjectMemberRequest
    {
        [Required]
        [NotEmptyGuid]
        public Guid? MemberId { get; set; }

        [Required]
        [EnumDataType(typeof(ProjectRole))]
        public ProjectRole? Role { get; set; }
    }
}
