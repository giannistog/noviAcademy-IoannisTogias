using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Queries.Players;

public class GetAllPlayersPersistenceCachingDecorator : IGetAllPlayersPersistence
{
    private readonly IGetAllPlayersPersistence _inner;
    private readonly ICache _cache;
    private const string CacheKey = "players:all";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    public GetAllPlayersPersistenceCachingDecorator(IGetAllPlayersPersistence inner, ICache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<IEnumerable<Player>> GetAll(CancellationToken ct = default)
    {
        if (_cache.TryGet(CacheKey, out IEnumerable<Player>? cached))
            return cached!;

        var players = (await _inner.GetAll(ct)).ToList();
        _cache.Set(CacheKey, (IEnumerable<Player>)players, CacheTtl);

        return players;
    }
}