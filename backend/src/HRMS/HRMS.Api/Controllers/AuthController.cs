using Asp.Versioning;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Authentication.Login;
using HRMS.Application.Features.Authentication.Logout;
using HRMS.Application.Features.Authentication.RefreshToken;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers;

[ApiController]
[ApiVersion(1)]
public sealed class AuthController : ApiController
{





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
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {

        var refreshToken = Request.Cookies["refreshToken"];

        var result = await dispatcher.SendAsync(
            new LogoutCommand(refreshToken),
            cancellationToken);

        return result.Match<IActionResult>(
            _ =>
            {
                DeleteRefreshTokenCookie();
                return NoContent();
            },
            Problem);
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
                SameSite = SameSiteMode.None,
                Expires = expiresAt,
                Path = "/api"
            });
    }

    private void DeleteRefreshTokenCookie()
    {
        Response.Cookies.Delete(
            "refreshToken",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/api"
            });
    }
}
