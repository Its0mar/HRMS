using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Abstractions.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        Task<ErrorOr<TResponse>> HandleAsync(
            TQuery query,
            CancellationToken cancellationToken);
    }
}
