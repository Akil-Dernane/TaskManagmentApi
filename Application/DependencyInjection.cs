

using Application.Members;
using Application.Projects.ArchiveProject;
using Application.Projects.CreateProject;
using Application.Projects.GetProjectById;
using Application.Projects.UpdateProject;
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
            return services;
        }
    }
}
