using Asp.Versioning;
using HRMS.Api.Contracts.Employess;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Employees.CreateEmployee;
using HRMS.Application.Features.Employees.GetEmployeeAccess;
using HRMS.Application.Features.Employees.GetEmployeeOptions;
using HRMS.Application.Features.Employees.GetEmployees;
using HRMS.Application.Features.Employees.UpdateEmployeeAccess;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class EmployeesController : ApiController
    {
        private readonly ICurrentUser _currentUser;

        public EmployeesController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Employees.Create)]
        public async Task<IActionResult> CreateAsync(
            CreateEmployeeCommand command,
            [FromServices] ICommandDispatcher dispatcher, 
            CancellationToken cancellationToken)
        {
           var result =  await dispatcher.SendAsync(
                command,
                cancellationToken);

            return result.Match<IActionResult>(
                response => Ok(result.Value),
                Problem);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Employees.View)]
        public async Task<IActionResult> GetAllAsync(
            [FromServices] IQueryHandler<GetEmployeesQuery, IReadOnlyList<GetEmployeesResponse>> handler, 
            CancellationToken cancellationToken)
        {
            var query = new GetEmployeesQuery();
            var result = await handler.HandleAsync(
                query,
                cancellationToken
                );

            return result.Match<IActionResult>(
                response => Ok(result.Value),
                Problem);
        }

        [HttpGet("options")]
        [Authorize(Policy = Permissions.Employees.View)]
        public async Task<IActionResult> GetOptionsAsync(
            [FromServices] IQueryHandler<GetEmployeeOptionsQuery, IReadOnlyList<EmployeeOptionResponse>> handler,
            CancellationToken cancellationToken)
        {
            var query = new GetEmployeeOptionsQuery();
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.Match<IActionResult>(
                response => Ok(result.Value),
                Problem);
        }


        [HttpGet("{employeeId:int}/access")]
        [Authorize(Policy = Permissions.Employees.Update)]
        public async Task<IActionResult> GetAccess(
            int employeeId,
            [FromServices] IQueryHandler<GetEmployeeAccessQuery, GetEmployeeAccessResponse> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(
                new GetEmployeeAccessQuery(employeeId, _currentUser.OrganizationId),
                cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }

        [HttpPut("{employeeId:int}/access")]
        [Authorize(Policy = Permissions.Employees.Update)]
        public async Task<IActionResult> UpdateAccess(
            int employeeId,
            UpdateEmployeeAccessRequest request,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken cancellationToken)
            {
                var command = new UpdateEmployeeAccessCommand(
                    employeeId,
                    request.Username,
                    request.RoleId);

                var result = await dispatcher.SendAsync(
                    command,
                    cancellationToken);

                return result.Match<IActionResult>(
                    _ => NoContent(),
                    Problem);
        }

    }
}
