namespace WorldRank;

using WorldRank.IPlayerRepository;

public class InMemoryPlayerRepository : IPlayerRepository
{
	private List<Player> players = new();


	public void AddPlayer(Player player){
		players.Add(player);
	}


	public Player? FindPlayer(int playerId)
	{
		return players.FirstOrDefault(p => p.Id == playerId);
	}


	public void DeletePlayer(int playerId)
	{
		Player player = FindPlayer(playerId);
		if (player != null)
		{
			players.Remove(player);
		}
	}
}