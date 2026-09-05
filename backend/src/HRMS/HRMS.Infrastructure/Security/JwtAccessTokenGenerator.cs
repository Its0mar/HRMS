using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Common.Settings;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS.Infrastructure.Security;

internal sealed class JwtAccessTokenGenerator(IOptions<JwtSettings> jwtOptions) : IAccessTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string Generate(User user, IReadOnlyCollection<string> permissions)
    {

        if (user.Id is null)
        {
            throw new InvalidOperationException("Cannot generate a token for a user without an ID.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("organization_id", user.OrganizationId.ToString()),
            new Claim("employee_id", user.EmployeeId.ToString() ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
