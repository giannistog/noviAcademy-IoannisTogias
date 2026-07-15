using WorldRank.Application.Abstractions;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Application.Commands;

public class DepositCommandHandler : ICommandHandler<DepositCommand, Wallet>
{
    private readonly IDepositPersistence _persistence;

    public DepositCommandHandler(IDepositPersistence persistence)
    {
        _persistence = persistence;
    }

    public async Task<Wallet> Handle(DepositCommand command, CancellationToken ct = default)
    {
        var wallet = await _persistence.GetById(command.WalletId, ct)
            ?? throw new WalletNotFoundException(command.WalletId, default);

        wallet.Deposit(command.Amount);
        await _persistence.Save(wallet, ct);

        return wallet;
    }
}