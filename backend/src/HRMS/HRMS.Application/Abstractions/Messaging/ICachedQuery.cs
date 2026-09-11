namespace HRMS.Application.Abstractions.Messaging
{
    public interface ICachedQuery
    {
        string CacheKey { get; }
        TimeSpan? Expiration => null;
    }
}
