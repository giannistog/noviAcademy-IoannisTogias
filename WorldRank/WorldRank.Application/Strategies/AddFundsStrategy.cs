using WorldRank.Domain.Entities;

namespace WorldRank.Application.Strategies;


public class AddFundsStrategy : IFundsStrategy
{
    public FundsOperation Operation => FundsOperation.Add;

    public void Execute(Wallet wallet, decimal amount) => wallet.Deposit(amount);
}
