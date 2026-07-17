using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets;

public class CreateWalletPersistenceCachingDecorator : ICreateWalletPersistence
{
    private readonly ICreateWalletPersistence _inner;
    private readonly ICache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    public CreateWalletPersistenceCachingDecorator(ICreateWalletPersistence inner, ICache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<Wallet> Persist(Wallet wallet, CancellationToken ct = default)
    {
        var saved = await _inner.Persist(wallet, ct);

        _cache.Set($"wallet:{saved.Id}", saved, CacheTtl);

        return saved;
    }
}