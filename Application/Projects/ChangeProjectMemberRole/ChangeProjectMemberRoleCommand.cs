using Domain.Enums;

namespace Application.Projects.ChangeProjectMemberRole
{
    public record ChangeProjectMemberRoleCommand(
        Guid ProjectId,
        Guid MemberId,
        ProjectRole NewRole);
}
