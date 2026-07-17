using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Persistence.Commands.Players;

public class CreatePlayerPersistence : ICreatePlayerPersistence
{
    private readonly WorldRankDbContext _context;

    public CreatePlayerPersistence(WorldRankDbContext context)
    {
        _context = context;
    }

    public async Task<Player> Persist(Player player, CancellationToken ct = default)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync(ct);

        return player;
    }
}