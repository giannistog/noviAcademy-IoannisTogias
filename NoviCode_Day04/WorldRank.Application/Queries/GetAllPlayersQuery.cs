using WorldRank.Application.Abstractions;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Queries;

public record GetAllPlayersQuery() : IQuery<IEnumerable<Player>>;