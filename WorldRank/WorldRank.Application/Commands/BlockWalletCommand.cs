using WorldRank.Application.Abstractions;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Commands;

public record BlockWalletCommand(int WalletId) : ICommand<Wallet>;