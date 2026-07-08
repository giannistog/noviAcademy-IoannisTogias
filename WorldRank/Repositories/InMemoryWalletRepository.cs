using NLog;

namespace WorldRank.Console
{
	public class InMemoryWalletRepository : IWalletRepository
	{
		private List<Player> _players;
        Logger logger = LogManager.GetCurrentClassLogger();

        public InMemoryWalletRepository(List<Player> players)
		{
			_players = players;
		}

		public void Add(Wallet wallet, int playerId)
		{
            //the bug mentioned in the 6th bullet: if player is null,
			//the method just does nothing caller does not know the wallet was never added.
            var player = _players.Where(item => item.Id == playerId).SingleOrDefault();

			if (player != null)
			{
				try
				{
					player.Wallets.Add(wallet.Currency, wallet);
                    logger.Info("Added {Currency} wallet to player {PlayerId}.", wallet.Currency, playerId);
                }catch (ArgumentException ex)
				//duplicate wallet exception was unhandled
				{
					logger.Error(ex, "Attempted to add {Currency} wallet to player {PlayerId}: Failed. Wallet of this Currency already exists", wallet.Currency, playerId);
					throw new WorldRank.Console.Exceptions.DuplicateWalletException("Player {playerId} already has a {wallet.Currency}");
				}
			}
			else
			{
                logger.Warn("Cannot add wallet: player {PlayerId} not found.", playerId);
            }
		}

		public List<Wallet> GetByPlayer(int playerId)
		{
			var wallets = _players.Where(item => item.Id == playerId).SelectMany(item => item.Wallets.Values);
			return wallets.ToList();
		}
	}
}