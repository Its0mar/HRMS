using ErrorOr;

namespace HRMS.Application.Abstractions.Messaging
{
    public interface ICommandDispatcher
    {
        Task<ErrorOr<TResponse>> SendAsync<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken);
    }
}
