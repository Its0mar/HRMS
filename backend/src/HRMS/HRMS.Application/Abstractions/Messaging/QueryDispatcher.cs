
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Application.Abstractions.Messaging
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ErrorOr<TResponse>> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
        {
            var queryType = query.GetType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));

            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            return await handler.HandleAsync((dynamic)query, cancellationToken);
        }
    }
}
