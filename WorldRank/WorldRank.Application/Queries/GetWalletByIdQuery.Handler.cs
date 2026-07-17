using WorldRank.Application.Abstractions;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Queries;

public class GetWalletByIdQueryHandler : IQueryHandler<GetWalletByIdQuery, Wallet?>
{
    private readonly IGetWalletByIdPersistence _persistence;

    public GetWalletByIdQueryHandler(IGetWalletByIdPersistence persistence)
    {
        _persistence = persistence;
    }

    public async Task<Wallet?> Handle(GetWalletByIdQuery query, CancellationToken ct = default)
    {
        return await _persistence.Get(query.WalletId, ct);
    }
}