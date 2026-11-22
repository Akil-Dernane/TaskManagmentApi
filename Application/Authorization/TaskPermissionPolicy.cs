using Domain.Enums;

namespace Application.Authorization
{
    public static class TaskPermissionPolicy
    {
        public static bool IsGranted(
            ProjectRole role,
            TaskPermission permission)
        {
            return permission switch
            {
                TaskPermission.ViewTasks =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager
                        or ProjectRole.Member,

                TaskPermission.CreateTasks =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager
                        or ProjectRole.Member,

                TaskPermission.UpdateAssignedTasks =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager
                        or ProjectRole.Member,

                TaskPermission.ChangeTaskPriority =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager,

                TaskPermission.AssignTasks =>
                    role is ProjectRole.Owner
                        or ProjectRole.Manager,

                _ => false
            };
        }
    }
}