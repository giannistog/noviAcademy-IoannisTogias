using WorldRank.Application.Abstractions;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Application.Commands;

public class BlockWalletCommandHandler : ICommandHandler<BlockWalletCommand, Wallet>
{
    private readonly IBlockWalletPersistence _persistence;

    public BlockWalletCommandHandler(IBlockWalletPersistence persistence)
    {
        _persistence = persistence;
    }

    public async Task<Wallet> Handle(BlockWalletCommand command, CancellationToken ct = default)
    {
        var wallet = await _persistence.GetById(command.WalletId, ct)
            ?? throw new WalletNotFoundException(command.WalletId, default);

        wallet.Block();
        await _persistence.Save(wallet, ct);

        return wallet;
    }
}