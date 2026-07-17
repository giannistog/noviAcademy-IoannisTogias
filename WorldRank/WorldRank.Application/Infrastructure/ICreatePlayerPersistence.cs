using WorldRank.Domain.Entities;

namespace WorldRank.Application.Infrastructure;

public interface ICreatePlayerPersistence
{
    Task<Player> Persist(Player player, CancellationToken ct = default);
}