using ErrorOr;

namespace HRMS.Application.Abstractions.Messaging
{
    public interface IQueryDispatcher
    {
        Task<ErrorOr<TResponse>> SendAsync<TResponse>(
            IQuery<TResponse> query,
            CancellationToken cancellationToken);
    }
}
