using Asp.Versioning;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.WorkSchedules.CreateWorkSchedules;
using HRMS.Application.Features.WorkSchedules.GetWorkSchedules;
using HRMS.Application.Features.WorkSchedules.UpdateWorkSchedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion(1)]
    public class WorkSchedulesController : ApiController
    {
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
                ture => Ok(),
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
    }
}
