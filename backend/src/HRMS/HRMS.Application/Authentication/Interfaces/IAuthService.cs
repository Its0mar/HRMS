using ErrorOr;
using HRMS.Application.Authentication.Dtos;
using HRMS.Domain.Entities;

namespace HRMS.Application.Authentication.Interfaces
{
    public interface IAuthService
    {
        public Task<ErrorOr<RegisterResponse>> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellationToken);
    }
}
