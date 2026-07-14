using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Repositories;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly ILogger<InMemoryPlayerRepository> _logger;

    private readonly List<Player> _players = new();

    public InMemoryPlayerRepository(ILogger<InMemoryPlayerRepository> logger)
    {
        _logger = logger;
    }

    public Task AddPlayer(Player player, CancellationToken ct = default)
    {
        _players.Add(player);
        _logger.LogInformation("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default)
    {
        return Task.FromResult<IEnumerable<Player>>(_players.ToList());
    }

    public Task DeletePlayer(int playerId, CancellationToken ct = default)
    {
        var player = _players.FirstOrDefault(item => item.Id == playerId);

        if (player is null)
        {
            _logger.LogWarning("Delete skipped: player {PlayerId} not found", playerId);
            return Task.CompletedTask;
        }

        _players.Remove(player);
        _logger.LogInformation("Player {PlayerId} deleted", playerId);
        return Task.CompletedTask;
    }

    public Task<Player?> FindPlayer(int playerId, CancellationToken ct = default)
    {
        return Task.FromResult(_players.FirstOrDefault(item => item.Id == playerId));
    }

    public Task<IEnumerable<IGrouping<int, Player>>> GroupPlayersByScore(CancellationToken ct = default)
    {
        var groups = _players
            .GroupBy(player => player.Score)
            .OrderByDescending(group => group.Key)
            .Cast<IGrouping<int, Player>>();

        return Task.FromResult(groups);
    }
}

/*public class InMemoryPlayerRepository : IPlayerRepository
{
	private readonly ILogger<InMemoryPlayerRepository> _logger;

	private readonly List<Player> _players = new();

	public InMemoryPlayerRepository(ILogger<InMemoryPlayerRepository> logger)
	{
		_logger = logger;
	}

	public void AddPlayer(Player player)
	{
		_players.Add(player);
		_logger.LogInformation("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
	}

	public IEnumerable<Player> GetAllPlayers()
	{
		// Return a copy so callers cannot mutate the repository's internal list.
		return _players.ToList();
	}

	public void DeletePlayer(int playerId)
	{
		var player = _players.FirstOrDefault(item => item.Id == playerId);

		if (player is null)
		{
			_logger.LogWarning("Delete skipped: player {PlayerId} not found", playerId);
			return;
		}

		_players.Remove(player);
		_logger.LogInformation("Player {PlayerId} deleted", playerId);
	}

	public Player? FindPlayer(int playerId)
	{
		return _players.FirstOrDefault(item => item.Id == playerId);
	}

	public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
	{
		return _players
			.GroupBy(player => player.Score)
			.OrderByDescending(group => group.Key);
	}
}*/
