using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HRMS.Infrastructure.Security;

internal sealed class JwtAccessTokenGenerator : IAccessTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtAccessTokenGenerator(IConfiguration configuration) => _configuration = configuration;

    public string Generate(User user, IReadOnlyCollection<string> permissions)
    {
        var keyValue = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT signing key is not configured.");
        var expiryValue = _configuration["Jwt:ExpireMinutes"] ?? throw new InvalidOperationException("JWT expiry is not configured.");

        if (user.Id is null)
        {
            throw new InvalidOperationException("Cannot generate a token for a user without an ID.");
        }

        if (!int.TryParse(expiryValue, out var expiryMinutes))
        {
            throw new InvalidOperationException("JWT expiry must be a valid integer.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("organization_id", user.OrganizationId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
