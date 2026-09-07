using Asp.Versioning;
using HRMS.Api.Contracts.Auth;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Authentication.ChangePassword;
using HRMS.Application.Features.Authentication.Login;
using HRMS.Application.Features.Authentication.Logout;
using HRMS.Application.Features.Authentication.RefreshToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers;

[ApiController]
[ApiVersion(1)]
public sealed class AuthController(ICommandDispatcher dispatcher) : ApiController
{
    private const string RefreshTokenCookieName = "refreshToken";

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Identifier, request.Password);
        var result = await dispatcher.SendAsync(command, cancellationToken);
        return result.Match<IActionResult>(
            response =>
            {
                SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpiresAt);
                return Ok(new AuthResponse(response.User, response.AccessToken));
            },
            Problem);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
        {
            return Unauthorized();
        }
        var result = await dispatcher.SendAsync(new RefreshTokenCommand(refreshToken), cancellationToken);
        return result.Match<IActionResult>(
            response =>
            {
                SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpiresAt);
                return Ok(new { response.AccessToken });
            },
            Problem);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        var result = await dispatcher.SendAsync(new LogoutCommand(refreshToken), cancellationToken);
        return result.Match<IActionResult>(
            _ =>
            {
                DeleteRefreshTokenCookie();
                return NoContent();
            },
            Problem);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(request.OldPassword, request.NewPassword);
        var result = await dispatcher.SendAsync(command, cancellationToken);
        return result.Match<IActionResult>(
            _ => Ok(new { message = "Password updated successfully." }),
            Problem);
    }

    private void SetRefreshTokenCookie(string token, DateTime expiresAt) =>
        Response.Cookies.Append(RefreshTokenCookieName, token, CreateCookieOptions(expiresAt));
    private void DeleteRefreshTokenCookie() =>
        Response.Cookies.Delete(RefreshTokenCookieName, CreateCookieOptions());
    private static CookieOptions CreateCookieOptions(DateTime? expiresAt = null) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Expires = expiresAt,
        Path = "/api"
    };
}