using WorldRank.Domain.Entities;

namespace WorldRank.Application.Infrastructure;

public interface ICreateWalletPersistence
{
    Task<Wallet> Persist(Wallet wallet, CancellationToken ct = default);
}