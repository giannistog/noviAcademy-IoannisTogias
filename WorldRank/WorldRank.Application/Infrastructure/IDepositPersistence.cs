using WorldRank.Domain.Entities;

namespace WorldRank.Application.Infrastructure;

public interface IDepositPersistence
{
    Task<Wallet?> GetById(int walletId, CancellationToken ct = default);
    Task Save(Wallet wallet, CancellationToken ct = default);
}