using WorldRank.Domain.Enums;

namespace WorldRank.Api.Dtos;

public record CreateWalletRequest(int PlayerId, Currency Currency, decimal InitialBalance);

public record WalletResponse(int Id, int PlayerId, Currency Currency, decimal Balance, bool IsBlocked)
{
    public static WalletResponse FromDomain(WorldRank.Domain.Entities.Wallet wallet)
        => new(wallet.Id, wallet.PlayerId, wallet.Currency, wallet.Balance, wallet.IsBlocked);
}

public record DepositRequest(Currency Currency, decimal Amount);