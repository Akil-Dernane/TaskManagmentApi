using API.ExceptionHandling;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });

            services.AddHealthChecks();

            services.AddProblemDetails();
            services.AddExceptionHandler<AuthenticationExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}
