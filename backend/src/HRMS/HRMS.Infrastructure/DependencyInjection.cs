using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Services;
using HRMS.Application.Common.Settings;
using HRMS.Domain.Entities.Common;
using HRMS.Infrastructure.Exceptions;
using HRMS.Infrastructure.Persistence;
using HRMS.Infrastructure.Repositories;
using HRMS.Infrastructure.Security;
using HRMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HRMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are missing in appsettings.json");

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.FromMinutes(1)
            });

        services.AddAuthorization(options =>
        {
            foreach (var permission in Permissions.All)
            {
                options.AddPolicy(
                    permission,
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(
                            "permission",
                            permission);
                    });
            }
        });

        services.AddHttpContextAccessor();
        services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(connectionString));
        services.AddScoped<ISqlExecutor, SqlExecutor>();

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();


        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPositionsRepository, PositionsRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IWorkScheduleRepository, WorkScheduleRepository>();
        services.AddScoped<IRolesRepository, RolesRepository>();
        services.AddScoped<IPermissionsRepository, PermissionsRepository>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IAttendanceCorrectionsRepository, AttendanceCorrectionsRepository>();
        services.AddScoped<ILeaveRepository, LeaveRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();

        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

        return services;
    }
}
