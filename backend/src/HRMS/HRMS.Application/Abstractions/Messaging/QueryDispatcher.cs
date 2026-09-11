
using ErrorOr;
using HRMS.Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Application.Abstractions.Messaging
{
    public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
    {
        public async Task<ErrorOr<TResponse>> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
        {
            var queryType = query.GetType();
            // 1. If Query implements ICachedQuery ➔ Intercept and handle via ICacheService
            if (query is ICachedQuery cachedQuery)
            {
                var cacheService = serviceProvider.GetRequiredService<ICacheService>();
                return await cacheService.GetOrCreateAsync(
                    cachedQuery.CacheKey,
                    async ct =>
                    {
                        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
                        dynamic handler = serviceProvider.GetRequiredService(handlerType);
                        return (ErrorOr<TResponse>)await handler.HandleAsync((dynamic)query, ct);
                    },
                    cachedQuery.Expiration,
                    cancellationToken);
            }
            // 2. Normal Query ➔ Execute DB Handler directly
            var normalHandlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
            dynamic normalHandler = serviceProvider.GetRequiredService(normalHandlerType);
            return await normalHandler.HandleAsync((dynamic)query, cancellationToken);
        }
    }
}
