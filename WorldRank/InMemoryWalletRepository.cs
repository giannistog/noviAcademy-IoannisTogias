
using WorldRank.IWalletRepository;

public class InMemoryWalletRepository : IWalletRepository
{
	private Dictionary<int, List<Wallet>> wallets = new();


	public void Add(Wallet wallet, int playerId){
		if (!wallets.ContainsKey(playerId))
		{
			wallets[playerId] = new List<Wallet>();
		}
		wallets[playerId].Add(wallet);
	}


	public IEnumerable<Wallet> GetByPlayer(int playerId){
		return wallets.Where(x => x.Key == playerId).SelectMany(x => x.Value);
	}
}