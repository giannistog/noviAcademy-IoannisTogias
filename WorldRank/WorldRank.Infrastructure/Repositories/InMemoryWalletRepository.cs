using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Infrastructure.Repositories;

public class InMemoryWalletRepository : IWalletRepository
{
    private readonly ILogger<InMemoryWalletRepository> _logger;

    private readonly List<Wallet> _wallets = new();

    public InMemoryWalletRepository(ILogger<InMemoryWalletRepository> logger)
    {
        _logger = logger;
    }

    public Task Add(Wallet wallet, CancellationToken ct = default)
    {
        var exists = _wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

        if (exists)
        {
            throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
        }

        _wallets.Add(wallet);
        _logger.LogInformation("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
        return Task.CompletedTask;
    }

    public Task<List<Wallet>> GetAllWalletsByPlayerId(int playerId, CancellationToken ct = default)
    {
        return Task.FromResult(_wallets.Where(item => item.PlayerId == playerId).ToList());
    }

    public async Task UpdateBalance(int playerId, Currency currency, decimal newBalance, CancellationToken ct = default)
    {
        var wallet = await GetWallet(playerId, currency, ct);
        wallet.SetBalance(newBalance);
        _logger.LogInformation("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
    }

    public async Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
    {
        var wallet = await GetWallet(playerId, currency, ct);
        wallet.Deposit(amount);
        _logger.LogInformation("Deposited {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
    }

    public async Task Withdraw(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
    {
        var wallet = await GetWallet(playerId, currency, ct);
        wallet.Withdraw(amount);
        _logger.LogInformation("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
    }

    public async Task Block(int playerId, Currency currency, CancellationToken ct = default)
    {
        var wallet = await GetWallet(playerId, currency, ct);
        wallet.Block();
        _logger.LogInformation("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
    }

    public async Task Unblock(int playerId, Currency currency, CancellationToken ct = default)
    {
        var wallet = await GetWallet(playerId, currency, ct);
        wallet.Unblock();
        _logger.LogInformation("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
    }

    public Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default)
    {
        var wallet = _wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency);

        if (wallet is null)
        {
            throw new WalletNotFoundException(playerId, currency);
        }

        return Task.FromResult(wallet);
    }

    public Task<Wallet[]> GetAll(CancellationToken ct = default)
    {
        return Task.FromResult(_wallets.ToArray());
    }

    public Task Save(CancellationToken ct = default)
    {
        // No-op: in-memory mutations are already "live" in the list.
        return Task.CompletedTask;
    }
    
}
