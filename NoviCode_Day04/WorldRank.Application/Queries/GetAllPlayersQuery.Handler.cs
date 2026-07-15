using WorldRank.Application.Abstractions;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Queries;

public class GetAllPlayersQueryHandler : IQueryHandler<GetAllPlayersQuery, IEnumerable<Player>>
{
    private readonly IGetAllPlayersPersistence _persistence;

    public GetAllPlayersQueryHandler(IGetAllPlayersPersistence persistence)
    {
        _persistence = persistence;
    }

    public async Task<IEnumerable<Player>> Handle(GetAllPlayersQuery query, CancellationToken ct = default)
    {
        return await _persistence.GetAll(ct);
    }
}