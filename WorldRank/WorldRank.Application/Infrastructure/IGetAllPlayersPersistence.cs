using WorldRank.Domain.Entities;

namespace WorldRank.Application.Infrastructure;

public interface IGetAllPlayersPersistence
{
    Task<IEnumerable<Player>> GetAll(CancellationToken ct = default);
}