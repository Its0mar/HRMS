using FluentValidation;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Authentication.Login;
using HRMS.Application.Features.Authentication.RefreshToken;
using HRMS.Application.Features.Authentication.RegisterOrganization;
using HRMS.Application.Features.Departments.CreateDepartment;
using HRMS.Application.Features.Departments.GetDepartments;
using HRMS.Application.Features.Departments.UpdateDepartment;
using HRMS.Application.Features.Employees.CreateEmployee;
using HRMS.Application.Features.Employees.GetEmployeeOptions;
using HRMS.Application.Features.Employees.GetEmployees;
using HRMS.Application.Features.Positions.CreatePosition;
using HRMS.Application.Features.Positions.GetPositions;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
         this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<
            RegisterOrganizationCommandValidator>();

        services.AddScoped<ICommandDispatcher, CommandDispatcher>();

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

        services.AddScoped<
            ICommandHandler<UpdateDepartmentCommand, bool>,
            UpdateDepartmentCommandHandler>();

        services.AddScoped<
            ICommandHandler<CreatePositionCommand, int>,
            CreatePositionCommandHandler>();

        services.AddScoped<
            IQueryHandler<GetPositionsQuery, List<GetPositionResponse>>,
            GetPositionsQueryHandler>();

        services.AddScoped<
            IQueryHandler<GetDepartmentsQuery, List<DepartmentListItem>>,
            GetDepartmentsQueryHandler>();

        services.AddScoped<
            ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>,
            RefreshTokenCommandHandler>();

        services.AddScoped<
            ICommandHandler<CreateEmployeeCommand, CreateEmployeeResponse>,
            CreateEmployeeHandler>();

        services.AddScoped<
            IQueryHandler<GetEmployeeOptionsQuery, IReadOnlyList<EmployeeOptionResponse>>,
            GetEmployeeOptionsHandler>();

        services.AddScoped<
            IQueryHandler<GetEmployeesQuery, IReadOnlyList<GetEmployeesResponse>>,
            GetEmployeesQueryHandler>();

        return services;
    }
}

