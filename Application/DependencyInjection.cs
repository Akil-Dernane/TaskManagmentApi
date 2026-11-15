

using Application.Members;
using Application.Projects.ArchiveProject;
using Application.Projects.CreateProject;
using Application.Projects.GetProjectById;
using Application.Projects.UpdateProject;
using Application.Tasks.ChangeTaskPriority;
using Application.Tasks.CompleteTask;
using Application.Tasks.CreateTaskItem;
using Application.Tasks.GetTaskItemById;
using Application.Tasks.GetTasksByProjectId;
using Application.Tasks.ReopenTask;
using Application.Tasks.StartTask;
using Application.Tasks.UpdateTaskItem;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<CreateProjectHandler>();
            services.AddTransient<GetProjectByIdHandler>();
            services.AddTransient<UpdateProjectHandler>();
            services.AddTransient<ArchiveProjectHandler>();

            services.AddScoped<IMemberService, MemberService>();

            services.AddTransient<GetTasksByProjectIdHandler>();
            services.AddTransient<CreateTaskItemHandler>();
            services.AddTransient<GetTaskItemByIdHandler>();
            services.AddTransient<UpdateTaskItemHandler>();
            services.AddTransient<StartTaskHandler>();
            services.AddTransient<CompleteTaskHandler>();
            services.AddTransient<ReopenTaskHandler>();
            services.AddTransient<ChangeTaskPriorityHandler>();

            return services;
        }
    }
}
