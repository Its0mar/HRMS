using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Abstractions.Persistence
{
    public sealed record OrganizationRegistrationResult(
        int OrganizationId,
        int OwnerUserId);
}
