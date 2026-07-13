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
    private readonly IMemoryCache _cache;

    public DBPlayerRepository(WorldRankDbContext context, ILogger<DBPlayerRepository> logger /*IMemoryCache cache*/)
    {
        _context = context;
        _logger = logger;
        //_cache = cache;
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

    /*public IEnumerable<Player> GetAllPlayers()
    {
        if (_cache.TryGetValue("AllPlayersKey", out IReadOnlyList<Player>? cached) && cached is not null)
        {
            _logger.LogInformation("Cache HIT  all players");
            return cached;
        }

        _logger.LogInformation("Cache MISS all players — loading from database");
        var players = _players.toList();

        _cache.Set("AllPlayersKey", players, TimeSpan.FromSeconds(60));

        return players;
    }*/

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

}