using System.Diagnostics;
using ErrorOr;
using HRMS.Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Abstractions.Messaging
{
    public class QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger) : IQueryDispatcher
    {
        public async Task<ErrorOr<TResponse>> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
        {
            var queryType = query.GetType();
            var queryName = queryType.Name;
            var stopwatch = Stopwatch.StartNew();

            logger.LogInformation("Executing Query {QueryName}", queryName);

            // 1. If Query implements ICachedQuery ➔ Intercept and handle via ICacheService
            if (query is ICachedQuery cachedQuery)
            {
                var cacheService = serviceProvider.GetRequiredService<ICacheService>();
                var cachedResult = await cacheService.GetOrCreateAsync(
                    cachedQuery.CacheKey,
                    async ct =>
                    {
                        logger.LogInformation("Cache miss for key {CacheKey}. Executing database query for {QueryName}", cachedQuery.CacheKey, queryName);
                        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
                        dynamic handler = serviceProvider.GetRequiredService(handlerType);
                        return (ErrorOr<TResponse>)await handler.HandleAsync((dynamic)query, ct);
                    },
                    cachedQuery.Expiration,
                    cancellationToken);

                stopwatch.Stop();
                logger.LogInformation("Query {QueryName} completed via Cache in {ElapsedMs}ms", queryName, stopwatch.ElapsedMilliseconds);
                return cachedResult;
            }

            // 2. Normal Query ➔ Execute DB Handler directly
            var normalHandlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
            dynamic normalHandler = serviceProvider.GetRequiredService(normalHandlerType);

            ErrorOr<TResponse> result = await normalHandler.HandleAsync((dynamic)query, cancellationToken);
            stopwatch.Stop();

            logger.LogInformation("Query {QueryName} completed via Database in {ElapsedMs}ms", queryName, stopwatch.ElapsedMilliseconds);
            return result;
        }
    }
}
