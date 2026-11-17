namespace Application.Projects.RemoveProjectMember
{
    public record RemoveProjectMemberCommand(
        Guid ProjectId,
        Guid MemberId);
}
