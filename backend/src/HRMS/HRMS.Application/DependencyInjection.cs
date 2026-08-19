using FluentValidation;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Attendance.ClockIn;
using HRMS.Application.Features.Attendance.ClockOut;
using HRMS.Application.Features.Attendance.GetEmployeeAttendance;
using HRMS.Application.Features.Authentication.Login;
using HRMS.Application.Features.Authentication.Logout;
using HRMS.Application.Features.Authentication.RefreshToken;
using HRMS.Application.Features.Departments.CreateDepartment;
using HRMS.Application.Features.Departments.GetDepartments;
using HRMS.Application.Features.Departments.UpdateDepartment;
using HRMS.Application.Features.Employees.Access.CreateEmployeeAccess;
using HRMS.Application.Features.Employees.Access.GetEmployeeAccess;
using HRMS.Application.Features.Employees.CreateEmployee;
using HRMS.Application.Features.Employees.GetEmployeeOptions;
using HRMS.Application.Features.Employees.GetEmployees;
using HRMS.Application.Features.Employees.UpdateEmployeeAccess;
using HRMS.Application.Features.Organizations.Registration;
using HRMS.Application.Features.Positions.CreatePosition;
using HRMS.Application.Features.Positions.GetPositions;
using HRMS.Application.Features.Roles.CreateRole;
using HRMS.Application.Features.Roles.GetRoleDetails;
using HRMS.Application.Features.Roles.GetRoles;
using HRMS.Application.Features.Roles.GetRolesOptions;
using HRMS.Application.Features.Roles.Permissions.GetPermissionOptions;
using HRMS.Application.Features.Roles.UpdateRole;
using HRMS.Application.Features.WorkSchedules.AssignEmployee;
using HRMS.Application.Features.WorkSchedules.CreateWorkSchedules;
using HRMS.Application.Features.WorkSchedules.GetWorkScheduleOptions;
using HRMS.Application.Features.WorkSchedules.GetWorkSchedules;
using HRMS.Application.Features.WorkSchedules.GetWorkScheduleWithDays;
using HRMS.Application.Features.WorkSchedules.UpdateWorkSchedule;
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
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

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

        services.AddScoped<
            ICommandHandler<CreateWorkScheduleCommand, int>,
            CreateWorkScheduleHandler>();

        services.AddScoped<
            ICommandHandler<UpdateWorkScheduleCommand, bool>,
            UpdateWorkScheduleHandler>();

        services.AddScoped<
            IQueryHandler<GetWorkSchedulesQuery, List<WorkScheduleResponse>>,
            GetWorkSchedulesQueryHandler>();

        services.AddScoped<
            IQueryHandler<GetWorkScheduleWithDaysQuery, WorkScheduleWithDaysResponse>,
            GetWorkScheduleWithDaysHandler>();

        services.AddScoped<
            IQueryHandler<GetRolesQuery, IReadOnlyList<GetRoleResponse>>,
            GetRolesHandler>();

        services.AddScoped<
            ICommandHandler<CreateRoleCommand, bool>,
            CreateRoleHandler>();

        services.AddScoped<
            IQueryHandler<GetPermissionOptionsQuery, IReadOnlyList<PermissionOptionResponse>>,
            GetPermissionOptionsHandler>();

        services.AddScoped<
            IQueryHandler<GetRoleByIdQuery, GetRoleDetailsResponse>,
            GetRoleByIdHandler>();
        services.AddScoped<
            ICommandHandler<UpdateRoleCommand, bool>,
            UpdateRoleHandler>();

        services.AddScoped<
            ICommandHandler<RegisterEmployeeCommand, int>,
            RegisterEmployeeHandler>();

        services.AddScoped<
            IQueryHandler<GetRolesOptionsQuery, IReadOnlyList<GetRolesOptionsResponse>>,
            GetRolesOptionsHandler>();

        services.AddScoped<
            IQueryHandler<GetEmployeeAccessQuery, GetEmployeeAccessResponse>,
            GetEmployeeAccessHandler>();

        services.AddScoped<
            ICommandHandler<UpdateEmployeeAccessCommand, bool>,
            UpdateEmployeeAccessHandler>();

        services.AddScoped<
            ICommandHandler<LogoutCommand, bool>,
            LogoutCommandHandler>();

        services.AddScoped<
            IQueryHandler<GetWorkScheduleOptionsQuery, IReadOnlyList<GetWorkScheduleOptionsResponse>>,
            GetWorkScheduleOptionsHandler>();

        services.AddScoped<
            ICommandHandler<AssignEmployeeCommand, bool>,
            AssignEmployeeHandler>();

        services.AddScoped<
            ICommandHandler<ClockInCommand, bool>,
            ClockInHandler>();

        services.AddScoped<
            ICommandHandler<ClockOutCommand, bool>,
            ClockOutHandler>();

        services.AddScoped<
            IQueryHandler<GetEmployeeAttendanceQuery, IReadOnlyList<GetEmployeeAttendanceResponse>>,
            GetEmployeeAttendanceHandler>();

        return services;
    }
}

