using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;

namespace WorldRank.Infrastructure.Caching;

public class MemoryCacheStore : ICache
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheStore> _logger;

    public MemoryCacheStore(IMemoryCache cache, ILogger<MemoryCacheStore> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public bool TryGet<T>(string key, out T? value)
    {
        var found = _cache.TryGetValue(key, out value);

        if (found)
            _logger.LogInformation("Cache HIT for key {Key}", key);
        else
            _logger.LogInformation("Cache MISS for key {Key}", key);

        return found;
    }

    public void Set<T>(string key, T value, TimeSpan ttl)
    {
        _cache.Set(key, value, ttl);
        _logger.LogInformation("Cache SET for key {Key} (ttl {Ttl}s)", key, ttl.TotalSeconds);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        _logger.LogInformation("Cache REMOVE for key {Key}", key);
    }
}