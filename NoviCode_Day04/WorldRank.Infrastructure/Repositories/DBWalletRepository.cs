using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Repositories
{

    public class DBWalletRepository : IWalletRepository
    {
        private readonly WorldRankDbContext _context;
        private readonly ILogger<DBWalletRepository> _logger;

        public DBWalletRepository(WorldRankDbContext context, ILogger<DBWalletRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(Wallet wallet, CancellationToken ct = default)
        {
            var exists = await _context.Wallets.AnyAsync(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency, ct);

            if (exists)
            {
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
            }

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
        }

        public async Task<List<Wallet>> GetAllWalletsByPlayerId(int playerId, CancellationToken ct = default)
        {
            return await _context.Wallets.Where(item => item.PlayerId == playerId).ToListAsync(ct);
        }

        public async Task UpdateBalance(int playerId, Currency currency, decimal newBalance, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.SetBalance(newBalance);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
        }

        public async Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Deposit(amount);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Deposited {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public async Task Withdraw(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Withdraw(amount);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public async Task Block(int playerId, Currency currency, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Block();
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
        }

        public async Task Unblock(int playerId, Currency currency, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Unblock();
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
        }

        public async Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default)
        {
            var wallet = await _context.Wallets.SingleOrDefaultAsync(item => item.PlayerId == playerId && item.Currency == currency, ct);

            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            return wallet;
        }

        public async Task<Wallet[]> GetAll(CancellationToken ct = default)
        {
            return await _context.Wallets.ToArrayAsync(ct);
        }

        public async Task Save(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
        /*
        private readonly WorldRankDbContext _context;
        private readonly ILogger<DBWalletRepository> _logger;

        public DBWalletRepository(WorldRankDbContext context, ILogger<DBWalletRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add(Wallet wallet)
        {
            var exists = _context.Wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

            if (exists)
            {
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
            }

            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            _logger.LogInformation("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
        }

        public List<Wallet> GetAllWalletsByPlayerId(int playerId)
        {
            return _context.Wallets.Where(item => item.PlayerId == playerId).ToList();
        }

        public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.SetBalance(newBalance);
            _context.SaveChanges();
            _logger.LogInformation("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
        }

        public void Deposit(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Deposit(amount);
            _context.SaveChanges();
            _logger.LogInformation("Deposited {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public void Withdraw(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Withdraw(amount);
            _context.SaveChanges();
            _logger.LogInformation("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public void Block(int playerId, Currency currency)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Block();
            _context.SaveChanges();
            _logger.LogInformation("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
        }

        public void Unblock(int playerId, Currency currency)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Unblock();
            _context.SaveChanges();
            _logger.LogInformation("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
        }

        public Wallet GetWallet(int playerId, Currency currency)
        {
            var wallet = _context.Wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency);

            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            return wallet;
        }

        public Wallet[] GetAll()
        {
            return _context.Wallets.ToArray();
        }
        public void Save() { _context.SaveChanges(); }
    }*/
    }
}