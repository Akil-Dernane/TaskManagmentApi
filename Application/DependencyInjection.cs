

using Application.Projects.CreateProject;
using Application.Projects.GetProjectById;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<CreateProjectHandler>();
            services.AddTransient<GetProjectByIdHandler>();
            return services;
        }
    }
}
