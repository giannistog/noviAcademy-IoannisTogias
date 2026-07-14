using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Services;

public class PlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICache _cache;

    public PlayerService(IPlayerRepository playerRepository, ICache cache)
    {
        _playerRepository = playerRepository;
        _cache = cache;
    }

    public async Task AddPlayer(CancellationToken ct = default)
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        Console.Write("Score: ");
        var scoreInput = Console.ReadLine();
        if (!int.TryParse(scoreInput, out var score))
        {
            Console.WriteLine("Score must be a whole number.");
            return;
        }

        var player = new Player(await GeneratePlayerId(ct), name);
        player.AddScore(score);
        await _playerRepository.AddPlayer(player, ct);
        Console.WriteLine("Player added successfully.");
    }

    public async Task ListPlayers(CancellationToken ct = default)
    {
        var all = (await _playerRepository.GetAllPlayers(ct)).ToList();

        if (all.Count == 0)
        {
            Console.WriteLine("No players registered.");
            return;
        }

        foreach (var player in all)
            Console.WriteLine(player);
    }

    public async Task ListPlayersByScore(CancellationToken ct = default)
    {
        var groups = (await _playerRepository.GroupPlayersByScore(ct)).ToList();

        if (groups.Count == 0)
        {
            Console.WriteLine("No players registered.");
            return;
        }

        foreach (var group in groups)
        {
            Console.WriteLine($"Score {group.Key}:");
            foreach (var player in group)
                Console.WriteLine($"  {player}");
        }
    }

    public async Task FindPlayerByName(CancellationToken ct = default)
    {
        Console.Write("Search by name: ");
        var term = Console.ReadLine() ?? string.Empty;

        var all = await _playerRepository.GetAllPlayers(ct);
        var player = all.FirstOrDefault(p => p.Name.Equals(term, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public async Task FindPlayerById(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var player = await _playerRepository.FindPlayer(playerId.Value, ct);

        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public async Task DeletePlayer(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        await _playerRepository.DeletePlayer(playerId.Value, ct);
        Console.WriteLine("Player deleted (if it existed).");
    }

    private async Task<int> GeneratePlayerId(CancellationToken ct = default)
    {
        var all = await _playerRepository.GetAllPlayers(ct);
        var existingIds = all.Select(p => p.Id).ToHashSet();

        int id;
        do
        {
            id = Random.Shared.Next(1, int.MaxValue);
        }
        while (existingIds.Contains(id));

        return id;
    }

    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);
    private static string PlayerCacheKey(int id) => $"player:{id}";
    private const string AllPlayersCacheKey = "players:all";

    public async Task<Player> CreatePlayer(string name, int score, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        var player = new Player(await GeneratePlayerId(ct), name);
        player.AddScore(score);
        await _playerRepository.AddPlayer(player, ct);

        // Write-through: populate the cache immediately, and invalidate the list cache.
        _cache.Set(PlayerCacheKey(player.Id), player, CacheTtl);
        _cache.Remove(AllPlayersCacheKey);

        return player;
    }

    public async Task<Player?> GetPlayerById(int playerId, CancellationToken ct = default)
    {
        var key = PlayerCacheKey(playerId);

        if (_cache.TryGet(key, out Player? cached))
            return cached;

        var player = await _playerRepository.FindPlayer(playerId, ct);

        if (player is not null)
            _cache.Set(key, player, CacheTtl);

        return player;
    }

    public async Task<IEnumerable<Player>> GetAllPlayersAsync(CancellationToken ct = default)
    {
        if (_cache.TryGet(AllPlayersCacheKey, out IEnumerable<Player>? cached))
            return cached!;

        var players = (await _playerRepository.GetAllPlayers(ct)).ToList();
        _cache.Set(AllPlayersCacheKey, (IEnumerable<Player>)players, CacheTtl);

        return players;
    }
}
    