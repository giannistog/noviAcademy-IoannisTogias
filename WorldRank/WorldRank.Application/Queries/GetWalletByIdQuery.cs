using WorldRank.Application.Abstractions;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Queries;

public record GetWalletByIdQuery(int WalletId) : IQuery<Wallet?>;