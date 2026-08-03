using Asp.Versioning;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.WorkSchedules.CreateWorkSchedules;
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
    }
}
