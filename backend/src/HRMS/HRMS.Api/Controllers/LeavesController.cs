using Asp.Versioning;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Leaves.LeaveBalances.GetLeaveBalances;
using HRMS.Application.Features.Leaves.LeaveRequests.SubmitLeaveRequest;
using HRMS.Application.Features.Leaves.LeaveType.CreateLeaveType;
using HRMS.Application.Features.Leaves.LeaveType.GetLeaveTypes;
using HRMS.Application.Features.Leaves.LeaveType.UpdateLeaveType;
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

        [Authorize]
        [HttpPut("leavetypes/update")]
        public async Task<IActionResult> Update(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize]
        [HttpGet("balances")]
        public async Task<IActionResult> GetMyLeaveBalances([FromQuery] int year, CancellationToken cancellationToken)
        {
            var query = new GetLeaveBalancesQuery(year, currentUser.EmployeeId, currentUser.OrganizationId);

            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize]
        [HttpPost("leaveRequests/apply")]
        public async Task<IActionResult> Apply(SubmitLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }
    }
}
