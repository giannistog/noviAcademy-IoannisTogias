using WorldRank.Application.Abstractions;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Commands;

public record DepositCommand(int WalletId, decimal Amount) : ICommand<Wallet>;