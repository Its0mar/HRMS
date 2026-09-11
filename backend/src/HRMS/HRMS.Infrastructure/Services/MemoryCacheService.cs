using HRMS.Application.Abstractions.Services;
using Microsoft.Extensions.Caching.Memory;

namespace HRMS.Infrastructure.Services
{
    internal sealed class MemoryCacheService(IMemoryCache cache) : ICacheService
    {
        public async Task<T?> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            if (cache.TryGetValue(key, out T? result) && result is not null)
            {
                return result;
            }

            var value = await factory(cancellationToken);
            if (value is not null)
            {
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(15)
                };

                cache.Set(key, value, options);
            }

            return value;
        }

        public void Remove(string key) => cache.Remove(key);

    }
}
