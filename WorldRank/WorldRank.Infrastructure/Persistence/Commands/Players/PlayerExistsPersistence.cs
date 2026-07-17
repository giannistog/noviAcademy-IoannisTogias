using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Persistence.Commands.Players;

public class PlayerExistsPersistence : IPlayerExistsPersistence
{
    private readonly WorldRankDbContext _context;

    public PlayerExistsPersistence(WorldRankDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Exists(int playerId, CancellationToken ct = default)
    {
        return await _context.Players.AnyAsync(p => p.Id == playerId, ct);
    }
}