using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Persistence.Queries.Wallets;

public class GetWalletByIdPersistence : IGetWalletByIdPersistence
{
    private readonly WorldRankDbContext _context;

    public GetWalletByIdPersistence(WorldRankDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet?> Get(int walletId, CancellationToken ct = default)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == walletId, ct);
    }
}