namespace WorldRank.Api.Dtos;

public record CreatePlayerRequest(string Name, int Score);

public record PlayerResponse(int Id, string Name, int Score)
{
    public static PlayerResponse FromDomain(WorldRank.Domain.Entities.Player player)
        => new(player.Id, player.Name, player.Score);
}