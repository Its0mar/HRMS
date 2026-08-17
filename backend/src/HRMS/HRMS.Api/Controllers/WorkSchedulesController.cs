using Asp.Versioning;
using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.WorkSchedules.AssignEmployee;
using HRMS.Application.Features.WorkSchedules.CreateWorkSchedules;
using HRMS.Application.Features.WorkSchedules.GetWorkScheduleOptions;
using HRMS.Application.Features.WorkSchedules.GetWorkSchedules;
using HRMS.Application.Features.WorkSchedules.GetWorkScheduleWithDays;
using HRMS.Application.Features.WorkSchedules.UpdateWorkSchedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class WorkSchedulesController : ApiController
    {
        private readonly ICurrentUser _currentUser;

        public WorkSchedulesController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(
            CreateWorkScheduleCommand command,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var result = await dispatcher.SendAsync(command, cancellationToken);
            return result.Match(
                workScheduleId => Ok(workScheduleId),
                errors => Problem(errors)
            );
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(
            UpdateWorkScheduleCommand command,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var result = await dispatcher.SendAsync(command, cancellationToken);
            return result.Match(
                ok => Ok(),
                errors => Problem(errors)
                );
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync(
            [FromServices] IQueryHandler<GetWorkSchedulesQuery, List<WorkScheduleResponse>> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(new GetWorkSchedulesQuery(), cancellationToken);

            return result.Match(
                workSchedules => Ok(workSchedules),
                errors => Problem(errors)
            );
        }

        [HttpPost("assignments")]
        [Authorize]
        public async Task<IActionResult> AssignEmployee(
            AssignEmployeeCommand command,
            [FromServices] ICommandHandler<AssignEmployeeCommand, bool> handler,
            CancellationToken cancellationToken
            )
        {
            var result = await handler.HandleAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpGet("options")]
        [Authorize]
        public async Task<IActionResult> GetOptions(
            [FromServices] IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetWorkScheduleOptionsQuery();
            var result = await dispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                errors => Problem(errors));
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            [FromRoute] int id,
            [FromServices] IQueryHandler<GetWorkScheduleWithDaysQuery, WorkScheduleWithDaysResponse> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(new GetWorkScheduleWithDaysQuery(id), cancellationToken);

            return result.Match(
                Ok,
                errors => Problem(errors)
            );
        }

    }
}
