using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Authentication.Login
{
    public sealed record AuthenticatedUserResponse(
        int Id,
        string Username,
        string Email,
        string FirstName,
        string LastName,
        int OrganizationId,
        IReadOnlyList<string> Permissions)
    {
        public static AuthenticatedUserResponse From(User user, IReadOnlyList<string> permissions)
        { 
            return new AuthenticatedUserResponse(
                user.Id!.Value,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.OrganizationId,
                permissions);
        }
    }
}
