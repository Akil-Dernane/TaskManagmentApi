using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Projects
{
    public class ChangeProjectMemberRoleRequest
    {
        [Required]
        [EnumDataType(typeof(ProjectRole))]
        public ProjectRole? Role { get; set; }
    }
}
