using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Repositories;

public class DBPlayerRepository : IPlayerRepository
{
    private readonly WorldRankDbContext _context;
    private readonly ILogger<DBPlayerRepository> _logger;

    public DBPlayerRepository(WorldRankDbContext context, ILogger<DBPlayerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddPlayer(Player player, CancellationToken ct = default)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
    }

    public async Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default)
    {
        return await _context.Players.AsNoTracking().ToListAsync(ct);
    }

    public async Task DeletePlayer(int playerId, CancellationToken ct = default)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, ct);

        if (player is null)
        {
            _logger.LogWarning("Delete skipped: player {PlayerId} not found", playerId);
            return;
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Player {PlayerId} deleted", playerId);
    }

    public async Task<Player?> FindPlayer(int playerId, CancellationToken ct = default)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, ct);
    }

    public async Task<IEnumerable<IGrouping<int, Player>>> GroupPlayersByScore(CancellationToken ct = default)
    {
        var players = await _context.Players.AsNoTracking().ToListAsync(ct);

        return players
            .GroupBy(player => player.Score)
            .OrderByDescending(group => group.Key);
    }
    /*
    private readonly WorldRankDbContext _context;
    private readonly ILogger<DBPlayerRepository> _logger;

    public DBPlayerRepository(WorldRankDbContext context, ILogger<DBPlayerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddPlayer(Player player, CancellationToken ct = default)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
    }

    public async Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default)
    {
        return await _context.Players.AsNoTracking().ToListAsync(ct);
    }

    public async Task DeletePlayer(int playerId, CancellationToken ct = default)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, ct);

        if (player is null)
        {
            _logger.LogWarning("Delete skipped: player {PlayerId} not found", playerId);
            return;
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Player {PlayerId} deleted", playerId);
    }

    public async Task<Player?> FindPlayer(int playerId, CancellationToken ct = default)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, ct);
    }

    public async Task<IEnumerable<IGrouping<int, Player>>> GroupPlayersByScore(CancellationToken ct = default)
    {
        var players = await _context.Players.AsNoTracking().ToListAsync(ct);

        return players
            .GroupBy(player => player.Score)
            .OrderByDescending(group => group.Key);
    }
}

public class DBPlayerRepository : IPlayerRepository
{
    private readonly WorldRankDbContext _context;
    private readonly ILogger<DBPlayerRepository> _logger;
    private readonly IMemoryCache _cache;

    public DBPlayerRepository(WorldRankDbContext context, ILogger<DBPlayerRepository> logger )
    {
        _context = context;
        _logger = logger;
        
    }

    public void AddPlayer(Player player)
    {
        _context.Players.Add(player);
        _context.SaveChanges();
        _logger.LogInformation("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
    }

    public IEnumerable<Player> GetAllPlayers()
    {
        return _context.Players.AsNoTracking().ToList();
    }

  
    public void DeletePlayer(int playerId)
    {
        var player = _context.Players.FirstOrDefault(p => p.Id == playerId);

        if (player is null)
        {
            _logger.LogWarning("Delete skipped: player {PlayerId} not found", playerId);
            return;
        }

        _context.Players.Remove(player);
        _context.SaveChanges();
        _logger.LogInformation("Player {PlayerId} deleted", playerId);
    }

    public Player? FindPlayer(int playerId)
    {
        return _context.Players.FirstOrDefault(p => p.Id == playerId);
    }

    public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
    {
        return _context.Players
            .AsNoTracking()
            .AsEnumerable()
            .GroupBy(player => player.Score)
            .OrderByDescending(group => group.Key);
    }
    */
}