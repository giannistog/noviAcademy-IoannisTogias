using WorldRank.Application.Abstractions;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Queries;

public class GetPlayerByIdQueryHandler : IQueryHandler<GetPlayerByIdQuery, Player?>
{
    private readonly IGetPlayerByIdPersistence _persistence;

    public GetPlayerByIdQueryHandler(IGetPlayerByIdPersistence persistence)
    {
        _persistence = persistence;
    }

    public async Task<Player?> Handle(GetPlayerByIdQuery query, CancellationToken ct = default)
    {
        return await _persistence.Get(query.PlayerId, ct);
    }
}