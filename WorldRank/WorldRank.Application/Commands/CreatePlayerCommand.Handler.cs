using WorldRank.Application.Abstractions;
using WorldRank.Application.Infrastructure;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Commands;

public class CreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand, Player>
{
    private readonly ICreatePlayerPersistence _persistence;

    public CreatePlayerCommandHandler(ICreatePlayerPersistence persistence)
    {
        _persistence = persistence;
    }

    public async Task<Player> Handle(CreatePlayerCommand command, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name cannot be empty.", nameof(command.Name));

        var player = new Player(Random.Shared.Next(1, int.MaxValue), command.Name);
        player.AddScore(command.Score);

        return await _persistence.Persist(player, ct);
    }
}