using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Permissions.GetPermissionOptions
{
    public sealed class GetPermissionOptionsHandler
    : IQueryHandler<GetPermissionOptionsQuery, IReadOnlyList<PermissionOptionResponse>>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public GetPermissionOptionsHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task<ErrorOr<IReadOnlyList<PermissionOptionResponse>>>
            HandleAsync(GetPermissionOptionsQuery query, CancellationToken cancellationToken)
        {
            var permissions = await _permissionsRepository.GetAllAsync(cancellationToken);

            return permissions.Select(permission => new PermissionOptionResponse(
                    permission.Id,
                    permission.Code,
                    permission.Description)).ToList();
        }
    }
}
