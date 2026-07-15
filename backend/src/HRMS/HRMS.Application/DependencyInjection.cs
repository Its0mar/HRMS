using HRMS.Application.Authentication.Interfaces;
using HRMS.Application.Authentication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
