using WorldRank.Domain.Entities;

namespace WorldRank.Application.Infrastructure;

public interface IGetPlayerByIdPersistence
{
    Task<Player?> Get(int playerId, CancellationToken ct = default);
}