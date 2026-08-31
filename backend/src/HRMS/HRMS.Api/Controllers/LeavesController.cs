using Asp.Versioning;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Leaves.LeaveBalances.GetLeaveBalances;
using HRMS.Application.Features.Leaves.LeaveRequests.ApproveLeaveRequest;
using HRMS.Application.Features.Leaves.LeaveRequests.CancelPending;
using HRMS.Application.Features.Leaves.LeaveRequests.GetMyLeaveRequests;
using HRMS.Application.Features.Leaves.LeaveRequests.GetOrganizationLeaveRequests;
using HRMS.Application.Features.Leaves.LeaveRequests.RejectLeaveRequest;
using HRMS.Application.Features.Leaves.LeaveRequests.SubmitLeaveRequest;
using HRMS.Application.Features.Leaves.LeaveTypes.CreateLeaveType;
using HRMS.Application.Features.Leaves.LeaveTypes.GetLeaveTypes;
using HRMS.Application.Features.Leaves.LeaveTypes.UpdateLeaveType;
using HRMS.Domain.Entities.Common;
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
        [Authorize(Policy = Permissions.LeaveTypes.Create)]
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

        [Authorize(Policy = Permissions.LeaveTypes.View)]
        [HttpGet("leavetypes/get")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetLeaveTypesQuery(currentUser.OrganizationId);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveTypes.Update)]
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

        [Authorize(Policy = Permissions.LeaveRequests.Submit)]
        [HttpPost("leaveRequests/apply")]
        public async Task<IActionResult> Apply([FromBody]SubmitLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize]
        [HttpGet("leaveRequests/list")]
        public async Task<IActionResult> MyRequests(CancellationToken cancellationToken)
        {
            var query = new GetMyLeaveRequestsQuery(currentUser.EmployeeId, currentUser.OrganizationId);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveRequests.View)]
        [HttpGet("leaveRequests/organization/list")]
        public async Task<IActionResult> OrganizationRequests([FromQuery] GetOrganizationLeaveRequestsQuery query, CancellationToken cancellationToken)
        {
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveRequests.ApproveAndReject)]
        [HttpPost("requests/approve")]
        public async Task<IActionResult> Approve(
            ApproveLeaveRequestCommand command,
            CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);
            return result.Match(_ => Ok(), Problem);
        }

        [Authorize(Policy = Permissions.LeaveRequests.ApproveAndReject)]
        [HttpPost("requests/reject")]
        public async Task<IActionResult> Reject(
            RejectLeaveRequestCommand command,
            CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);
            return result.Match(_ => Ok(), Problem);
        }

        [Authorize]
        [HttpPut("requests/{id:int}/cancel")]
        public async Task<IActionResult> Cancel(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new CancelPendingCommand(id, currentUser.EmployeeId, currentUser.OrganizationId);
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(_ => Ok(), Problem);
        }

    }
}
