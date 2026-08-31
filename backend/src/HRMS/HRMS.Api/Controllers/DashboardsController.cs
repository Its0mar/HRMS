using Asp.Versioning;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Dashboard.EmployeeDasboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class DashboardsController(
        IQueryDispatcher queryDispatcher,
        ICurrentUser currentUser
        ) : ApiController
    {
        [Authorize]
        [HttpGet("employee")]
        public async Task<IActionResult> GetEmployeeDashboard(CancellationToken cancellationToken)
        {
            var query = new EmployeeDasboardQuery(currentUser.EmployeeId, currentUser.OrganizationId, DateTime.UtcNow.Year);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }
    }
}
