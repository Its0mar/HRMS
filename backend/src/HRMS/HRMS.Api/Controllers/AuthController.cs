using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Authentication.Login;
using HRMS.Application.Features.Authentication.RefreshToken;
using HRMS.Application.Features.Authentication.RegisterOrganization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController : ApiController
{
    [HttpPost("organizations")]
    public async Task<IActionResult> RegisterOrganization(
        RegisterOrganizationCommand command,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(
            command,
            cancellationToken);

        return result.Match<IActionResult>(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginCommand command,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(
            command,
            cancellationToken);

        return result.Match<IActionResult>(
            response =>
            {
                SetRefreshTokenCookie(
                    response.RefreshToken,
                    response.RefreshTokenExpiresAt);

                return Ok(new
                {
                    response.User,
                    response.AccessToken
                });
            },
            Problem);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized();
        }

        var result = await dispatcher.SendAsync(new RefreshTokenCommand(refreshToken), cancellationToken);

        return result.Match<IActionResult>(
            response =>
            {
                SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpiresAt);
                return Ok(new
                {
                    response.AccessToken
                });
            },
            Problem);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromServices] IRefreshTokenRepository repository,
        [FromServices] IRefreshTokenGenerator generator,
        CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            await repository.RevokeAsync(
                generator.Hash(refreshToken),
                DateTime.UtcNow,
                cancellationToken);
        }

        Response.Cookies.Delete("refrehToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api"
        });

        return NoContent();
    }


    private void SetRefreshTokenCookie(string token, DateTime expiresAt)
    {
        Response.Cookies.Append(
            "refreshToken",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiresAt,
                Path = "/api"
            });
    }
}
