

using Application.Authentication.Login;
using Application.Authentication.Logout;
using Application.Authentication.Refresh;
using Application.Authentication.Register;
using Application.Members;
using Application.Projects.AddProjectMember;
using Application.Projects.ArchiveProject;
using Application.Projects.ChangeProjectMemberRole;
using Application.Projects.CreateProject;
using Application.Projects.GetProjectById;
using Application.Projects.RemoveProjectMember;
using Application.Projects.UpdateProject;
using Application.Tasks.AssignTask;
using Application.Tasks.ChangeTaskPriority;
using Application.Tasks.CompleteTask;
using Application.Tasks.CreateTaskItem;
using Application.Tasks.GetTaskItemById;
using Application.Tasks.GetTasksByProjectId;
using Application.Tasks.ReopenTask;
using Application.Tasks.StartTask;
using Application.Tasks.UnassignTask;
using Application.Tasks.UpdateTaskItem;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<RegisterUserHandler>();
            services.AddScoped<LoginUserHandler>();
            services.AddScoped<RefreshTokenHandler>();

            services.AddScoped<LogoutHandler>();
            services.AddScoped<LogoutAllHandler>();

            services.AddTransient<CreateProjectHandler>();
            services.AddTransient<GetProjectByIdHandler>();
            services.AddTransient<UpdateProjectHandler>();
            services.AddTransient<ArchiveProjectHandler>();
            services.AddTransient<AddProjectMemberHandler>();
            services.AddTransient<RemoveProjectMemberHandler>();
            services.AddTransient<ChangeProjectMemberRoleHandler>();

            services.AddScoped<IMemberService, MemberService>();

            services.AddTransient<GetTasksByProjectIdHandler>();
            services.AddTransient<CreateTaskItemHandler>();
            services.AddTransient<GetTaskItemByIdHandler>();
            services.AddTransient<UpdateTaskItemHandler>();
            services.AddTransient<StartTaskHandler>();
            services.AddTransient<CompleteTaskHandler>();
            services.AddTransient<ReopenTaskHandler>();
            services.AddTransient<ChangeTaskPriorityHandler>();
            services.AddTransient<AssignTaskHandler>();
            services.AddTransient<UnassignTaskHandler>();

            return services;
        }
    }
}
