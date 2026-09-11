
using ErrorOr;
using HRMS.Application.Abstractions.Services;

namespace HRMS.Application.Abstractions.Messaging
{
    public sealed class CachedQueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> inner,
        ICacheService cacheService) 
        : IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>, ICachedQuery
    {
        public async Task<ErrorOr<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken)
        {
            var result =  await cacheService.GetOrCreateAsync(
                query.CacheKey,
                ct => inner.HandleAsync(query, cancellationToken),
                query.Expiration,
                cancellationToken);

            return result;
        }
    }
}
