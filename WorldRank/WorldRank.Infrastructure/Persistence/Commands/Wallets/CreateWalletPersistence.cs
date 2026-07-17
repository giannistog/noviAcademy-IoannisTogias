using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Exceptions;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets;

public class CreateWalletPersistence : ICreateWalletPersistence
{
    private readonly WorldRankDbContext _context;

    public CreateWalletPersistence(WorldRankDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet> Persist(Wallet wallet, CancellationToken ct = default)
    {
        var exists = _context.Wallets.Any(w => w.PlayerId == wallet.PlayerId && w.Currency == wallet.Currency);

        if (exists)
            throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);

        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync(ct);

        return wallet;
    }
}