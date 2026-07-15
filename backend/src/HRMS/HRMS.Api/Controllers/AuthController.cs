using HRMS.Application.Authentication.Dtos;
using HRMS.Application.Authentication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("create-org")]
        public async Task<IActionResult> OrganizationRegister(RegisterRequest request, CancellationToken ct)
        {
            var result = await _authService.RegisterAsync(request, ct);

            return result.Match<IActionResult>(
                value => Ok("organization created successfully"),
                error => BadRequest(new
                {
                    error = error.First().Description ?? "a problem occured"
                })
                );
        }
    }
}