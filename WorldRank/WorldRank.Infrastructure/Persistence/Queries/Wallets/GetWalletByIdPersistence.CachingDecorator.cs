using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Queries.Wallets;

public class GetWalletByIdPersistenceCachingDecorator : IGetWalletByIdPersistence
{
    private readonly IGetWalletByIdPersistence _inner;
    private readonly ICache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    public GetWalletByIdPersistenceCachingDecorator(IGetWalletByIdPersistence inner, ICache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<Wallet?> Get(int walletId, CancellationToken ct = default)
    {
        var key = $"wallet:{walletId}";

        if (_cache.TryGet(key, out Wallet? cached))
            return cached;

        var wallet = await _inner.Get(walletId, ct);

        if (wallet is not null)
            _cache.Set(key, wallet, CacheTtl);

        return wallet;
    }
}