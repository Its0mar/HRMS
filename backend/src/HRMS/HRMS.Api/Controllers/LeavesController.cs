using Asp.Versioning;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Leaves.LeaveType.CreateLeaveType;
using HRMS.Application.Features.Leaves.LeaveType.GetLeaveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{

    [ApiController]
    [ApiVersion(1)]
    public class LeavesController(
         ICommandDispatcher commandDispatcher,
         IQueryDispatcher queryDispatcher,
         ICurrentUser currentUser) : ApiController
    {
        [Authorize]
        [HttpPost("leaveTypes/create")]
        public async Task<IActionResult> Create(
            CreateLeaveTypeCommand command,
            CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize]
        [HttpGet("leavetypes/get")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetLeaveTypesQuery(currentUser.OrganizationId);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }
    }
}
