using WorldRank.Domain.Entities;

namespace WorldRank.Application.Infrastructure;

public interface IGetWalletByIdPersistence
{
    Task<Wallet?> Get(int walletId, CancellationToken ct = default);
}