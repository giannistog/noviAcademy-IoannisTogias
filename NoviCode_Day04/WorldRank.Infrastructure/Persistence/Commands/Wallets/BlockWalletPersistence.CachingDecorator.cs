using WorldRank.Application.Infrastructure;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets;

public class BlockWalletPersistenceCachingDecorator : IBlockWalletPersistence
{
    private readonly IBlockWalletPersistence _inner;
    private readonly ICache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);

    public BlockWalletPersistenceCachingDecorator(IBlockWalletPersistence inner, ICache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public Task<Wallet?> GetById(int walletId, CancellationToken ct = default)
    {
        return _inner.GetById(walletId, ct);
    }

    public async Task Save(Wallet wallet, CancellationToken ct = default)
    {
        await _inner.Save(wallet, ct);
        _cache.Set($"wallet:{wallet.Id}", wallet, CacheTtl);
    }
}