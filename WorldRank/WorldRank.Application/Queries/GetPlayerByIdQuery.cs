using WorldRank.Application.Abstractions;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Queries;

public record GetPlayerByIdQuery(int PlayerId) : IQuery<Player?>;