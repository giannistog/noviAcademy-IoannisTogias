namespace WorldRank.Application.Infrastructure;

public interface IPlayerExistsPersistence
{
    Task<bool> Exists(int playerId, CancellationToken ct = default);
}