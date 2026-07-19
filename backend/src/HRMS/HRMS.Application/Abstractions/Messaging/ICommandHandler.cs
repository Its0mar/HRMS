using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Abstractions.Messaging
{
    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<ErrorOr<TResponse>> HandleAsync(
            TCommand command,
            CancellationToken cancellationToken);
    }
}
