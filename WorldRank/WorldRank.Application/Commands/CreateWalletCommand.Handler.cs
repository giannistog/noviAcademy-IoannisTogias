using WorldRank.Application.Abstractions;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Application.Commands;

public class CreateWalletCommandHandler : ICommandHandler<CreateWalletCommand, Wallet>
{
    private readonly ICreateWalletPersistence _persistence;
    private readonly IPlayerExistsPersistence _playerExists;

    public CreateWalletCommandHandler(ICreateWalletPersistence persistence, IPlayerExistsPersistence playerExists)
    {
        _persistence = persistence;
        _playerExists = playerExists;
    }

    public async Task<Wallet> Handle(CreateWalletCommand command, CancellationToken ct = default)
    {
        if (!await _playerExists.Exists(command.PlayerId, ct))
            throw new PlayerNotFoundException(command.PlayerId);

        var wallet = new Wallet(Random.Shared.Next(1, int.MaxValue), command.PlayerId, command.Currency, command.InitialBalance);

        return await _persistence.Persist(wallet, ct);
    }
}