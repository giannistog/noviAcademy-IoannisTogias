using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;

namespace WorldRank.Application.Interfaces;

public interface IWalletRepository
{
    Task Add(Wallet wallet, CancellationToken ct = default);

    Task<List<Wallet>> GetAllWalletsByPlayerId(int playerId, CancellationToken ct = default);

    Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default);

    Task UpdateBalance(int playerId, Currency currency, decimal newBalance, CancellationToken ct = default);

    Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default);

    Task Withdraw(int playerId, Currency currency, decimal amount, CancellationToken ct = default);

    Task Block(int playerId, Currency currency, CancellationToken ct = default);

    Task Unblock(int playerId, Currency currency, CancellationToken ct = default);

    Task<Wallet[]> GetAll(CancellationToken ct = default);

    Task Save(CancellationToken ct = default);
    /*void Add(Wallet wallet);

	List<Wallet> GetAllWalletsByPlayerId(int playerId);

	Wallet GetWallet(int playerId, Currency currency);

	void UpdateBalance(int playerId, Currency currency, decimal newBalance);

	void Deposit(int playerId, Currency currency, decimal amount);

	void Withdraw(int playerId, Currency currency, decimal amount);

	void Block(int playerId, Currency currency);

	void Unblock(int playerId, Currency currency);


    Wallet[] GetAll();

    void Save(); //new method to save data in the database, retainable even after solution closes

	*/
}
