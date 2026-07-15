using WorldRank.Application.Abstractions;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Commands;

public record CreatePlayerCommand(string Name, int Score) : ICommand<Player>;