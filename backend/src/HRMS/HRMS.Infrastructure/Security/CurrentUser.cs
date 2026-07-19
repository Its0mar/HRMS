using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HRMS.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace HRMS.Infrastructure.Security;

internal sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;


    public int Id => ReadRequiredIntegerClaim(JwtRegisteredClaimNames.Sub, ClaimTypes.NameIdentifier);

    public int OrganizationId => ReadRequiredIntegerClaim("organization_id");

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    private int ReadRequiredIntegerClaim(params string[] claimTypes)
    {
        foreach (var claimType in claimTypes)
        {
            var value = User?.FindFirst(claimType)?.Value;

            if (int.TryParse(value, out var result))
            {
                return result;
            }
        }

        throw new UnauthorizedAccessException("The required authenticated-user claim is missing.");
    }
}
