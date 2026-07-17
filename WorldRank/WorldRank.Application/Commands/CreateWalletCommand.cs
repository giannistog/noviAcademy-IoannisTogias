using WorldRank.Application.Abstractions;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;

namespace WorldRank.Application.Commands;

public record CreateWalletCommand(int PlayerId, Currency Currency, decimal InitialBalance) : ICommand<Wallet>;