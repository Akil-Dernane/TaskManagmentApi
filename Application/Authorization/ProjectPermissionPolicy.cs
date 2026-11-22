using Domain.Enums;

namespace Application.Authorization
{
    public static class ProjectPermissionPolicy
    {
        public static bool IsGranted(
            ProjectRole role,
            ProjectPermission permission)
        {
            return permission switch
            {
                ProjectPermission.ViewProject =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager
                        or ProjectRole.Member,

                ProjectPermission.UpdateProject =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager,

                ProjectPermission.ArchiveProject =>
                    role == ProjectRole.Owner,

                ProjectPermission.ManageMembers =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager,

                ProjectPermission.TransferOwnership =>
                    role == ProjectRole.Owner,

                _ => false
            };
        }
    }
}