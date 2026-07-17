using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Queries.Players;

public class GetPlayerByIdPersistenceCachingDecorator : IGetPlayerByIdPersistence
{
    private readonly IGetPlayerByIdPersistence _inner;
    private readonly ICache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    public GetPlayerByIdPersistenceCachingDecorator(IGetPlayerByIdPersistence inner, ICache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<Player?> Get(int playerId, CancellationToken ct = default)
    {
        var key = $"player:{playerId}";

        if (_cache.TryGet(key, out Player? cached))
            return cached;

        var player = await _inner.Get(playerId, ct);

        if (player is not null)
            _cache.Set(key, player, CacheTtl);

        return player;
    }
}