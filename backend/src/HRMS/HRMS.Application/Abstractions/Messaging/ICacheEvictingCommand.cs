namespace HRMS.Application.Abstractions.Messaging;

public interface ICacheEvictingCommand
{
    string CacheKeyToEvict { get; }
}
