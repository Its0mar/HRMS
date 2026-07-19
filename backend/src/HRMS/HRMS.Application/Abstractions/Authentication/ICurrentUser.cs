using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Abstractions.Authentication
{
    public interface ICurrentUser
    {
        public int Id { get; }
        public int OrganizationId { get; }
        public bool IsAuthenticated { get; }
    }
}
