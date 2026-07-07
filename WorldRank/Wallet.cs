namespace WorldRank;

public class Wallet
{
    public decimal Balance { get; private set; }
    public Currency Currency { get; private set; }
    public bool IsBlocked { get; private set; }


    public Wallet(Currency currency, decimal balance)
    {
        if (balance < 0)
            throw new ArgumentException("Balance cannot be negative");

        Currency = currency;
        Balance = balance;
        IsBlocked = false;
    }


    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Invalid amount");
        Balance += amount;
    }


    public void Withdraw(decimal amount)
    {
        if (Balance - amount < 0)
            throw new InvalidOperationException("Insufficient Funfs");
        Balance -= amount;
    }
}