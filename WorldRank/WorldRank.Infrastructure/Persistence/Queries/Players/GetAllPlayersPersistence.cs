using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Persistence.Queries.Players;

public class GetAllPlayersPersistence : IGetAllPlayersPersistence
{
    private readonly WorldRankDbContext _context;

    public GetAllPlayersPersistence(WorldRankDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Player>> GetAll(CancellationToken ct = default)
    {
        return await _context.Players.AsNoTracking().ToListAsync(ct);
    }
}