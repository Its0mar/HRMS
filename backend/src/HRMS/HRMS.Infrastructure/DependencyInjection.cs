using HRMS.Application.Authentication.Interfaces;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Repositories;
using HRMS.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDbConnectionFactory>(
                new DbConnectionFactory(
                    configuration.GetConnectionString("DefaultConnection")!
                ));

            services.AddScoped<ISqlExecutor, SqlExecutor>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
