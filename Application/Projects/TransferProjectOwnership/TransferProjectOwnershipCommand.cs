namespace Application.Projects.TransferProjectOwnership
{
    public record TransferProjectOwnershipCommand(
        Guid ProjectId,
        Guid NewOwnerMemberId);
}