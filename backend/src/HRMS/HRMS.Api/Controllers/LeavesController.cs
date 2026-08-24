using Asp.Versioning;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Leaves.LeaveType.CreateLeaveType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{

    [ApiController]
    [ApiVersion(1)]
    public class LeavesController(
         ICommandDispatcher commandDispatcher,
         IQueryDispatcher queryDispatcher) : ApiController
    {
        [Authorize]
        [HttpPost("leaveTypes/create")]
        public async Task<IActionResult> ClockIn(
            CreateLeaveTypeCommand command,
            CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }
    }
}
