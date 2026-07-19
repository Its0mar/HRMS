using FluentValidation;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Authentication.Login;
using HRMS.Application.Features.Authentication.RegisterOrganization;
using HRMS.Application.Features.Departments.CreateDepartment;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
         this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<
            RegisterOrganizationCommandValidator>();

        services.AddScoped<
            ICommandHandler<
                RegisterOrganizationCommand,
                RegisterOrganizationResponse>,
            RegisterOrganizationCommandHandler>();

        services.AddScoped<
            ICommandHandler<LoginCommand, LoginResponse>,
            LoginCommandHandler>();

        services.AddScoped<
            ICommandHandler<
                CreateDepartmentCommand,
                CreateDepartmentResponse>,
            CreateDepartmentCommandHandler>();

        return services;
    }
}

