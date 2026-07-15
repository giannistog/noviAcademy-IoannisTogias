using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Persistence.Commands.Wallets;

public class DepositPersistence : IDepositPersistence
{
    private readonly WorldRankDbContext _context;

    public DepositPersistence(WorldRankDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet?> GetById(int walletId, CancellationToken ct = default)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == walletId, ct);
    }

    public async Task Save(Wallet wallet, CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}