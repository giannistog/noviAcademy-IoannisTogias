using NLog;
using WorldRank.Console.Enums;
using WorldRank.Console.Exceptions;

namespace WorldRank.Console
{
	public class Wallet
	{
        Logger logger = LogManager.GetCurrentClassLogger();
        public decimal Balance { get; private set; }
		public Currency Currency;
		public bool IsBlocked;

		public Wallet(decimal balance, Currency currency, bool isBlocked)
		{

           
			Balance = balance;
			Currency = currency;
			IsBlocked = isBlocked;

            logger.Debug("Created wallet: {Wallet}", this);
        }

		public void SetBalance(decimal balance)
		{
			if (balance < 0)
			{
				logger.Error("Attempted to set negative balance ({balance}) on ({Currency}) currency wallet.", balance, Currency);
				throw new WorldRank.Console.Exceptions.InsufficientFundsException("Balance cannot be negative.");
			}
			Balance = balance;
		}

		public override string ToString()
		{
			return "Balance -> " + Balance + " Currency ->" + Currency + " IsBlocked -> " + IsBlocked;
		}
	}
}