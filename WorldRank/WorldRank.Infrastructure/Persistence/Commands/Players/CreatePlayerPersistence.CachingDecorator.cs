using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Commands.Players;

public class CreatePlayerPersistenceCachingDecorator : ICreatePlayerPersistence
{
    private readonly ICreatePlayerPersistence _inner;
    private readonly ICache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    public CreatePlayerPersistenceCachingDecorator(ICreatePlayerPersistence inner, ICache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<Player> Persist(Player player, CancellationToken ct = default)
    {
        var saved = await _inner.Persist(player, ct);

        _cache.Set($"player:{saved.Id}", saved, CacheTtl);
        _cache.Remove("players:all");

        return saved;
    }
}