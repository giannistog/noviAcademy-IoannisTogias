using WorldRank.Domain.Entities;

namespace WorldRank.Application.Interfaces;

public interface IPlayerRepository
{
    Task AddPlayer(Player player, CancellationToken ct = default);

    Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default);

    Task DeletePlayer(int playerId, CancellationToken ct = default);

    Task<Player?> FindPlayer(int playerId, CancellationToken ct = default);

    Task<IEnumerable<IGrouping<int, Player>>> GroupPlayersByScore(CancellationToken ct = default);
    
}