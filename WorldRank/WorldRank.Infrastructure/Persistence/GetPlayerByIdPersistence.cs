using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Persistence.Queries.Players;

public class GetPlayerByIdPersistence : IGetPlayerByIdPersistence
{
    private readonly WorldRankDbContext _context;

    public GetPlayerByIdPersistence(WorldRankDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> Get(int playerId, CancellationToken ct = default)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, ct);
    }
}