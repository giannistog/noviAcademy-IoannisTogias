using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;
using WorldRank.Application.Interfaces;

namespace WorldRank.Application.Services;

public class WalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<WalletService> _logger;
    private readonly IReadOnlyDictionary<FundsOperation, IFundsStrategy> _fundsStrategies;
    private readonly ICache _cache;

    public WalletService(
        IWalletRepository walletRepository,
        IPlayerRepository playerRepository,
        IEnumerable<IFundsStrategy> strategies,
        ILogger<WalletService> logger,
        ICache cache)
    {
        _walletRepository = walletRepository;
        _playerRepository = playerRepository;
        _logger = logger;
        _fundsStrategies = strategies.ToDictionary(strategy => strategy.Operation);
        _cache = cache;
    }

    public async Task AddWalletToPlayer(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var balance = Prompts.PromptAmount("Initial balance");
        if (balance is null)
            return;

        try
        {
            if (await _playerRepository.FindPlayer(playerId.Value, ct) is null)
                throw new PlayerNotFoundException(playerId.Value);

            var wallet = new Wallet(await GenerateWalletId(ct), playerId.Value, currency.Value, balance.Value);
            await _walletRepository.Add(wallet, ct);
            Console.WriteLine("Wallet added successfully.");
        }
        catch (PlayerNotFoundException ex)
        {
            _logger.LogWarning(ex, "Could not add wallet, player {PlayerId} not found", playerId);
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (WalletException ex)
        {
            _logger.LogWarning(ex, "Could not add wallet for player {PlayerId} in {Currency}", playerId, currency);
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task GetWalletsOfPlayer(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var wallets = await _walletRepository.GetAllWalletsByPlayerId(playerId.Value, ct);

        if (wallets.Count == 0)
        {
            Console.WriteLine("No wallets found for this player.");
            return;
        }

        foreach (var wallet in wallets)
            Console.WriteLine($"Wallet Number {wallets.IndexOf(wallet)} {wallet}");
    }

    public async Task DepositToWallet(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var amount = Prompts.PromptAmount("Amount to deposit");
        if (amount is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Deposit(playerId.Value, currency.Value, amount.Value, ct);
            Console.WriteLine("Deposit successful.");
        });
    }

    public async Task WithdrawFromWallet(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var amount = Prompts.PromptAmount("Amount to withdraw");
        if (amount is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Withdraw(playerId.Value, currency.Value, amount.Value, ct);
            Console.WriteLine("Withdrawal successful.");
        });
    }

    public async Task BlockWallet(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Block(playerId.Value, currency.Value, ct);
            Console.WriteLine("Wallet blocked.");
        });
    }

    public async Task UnblockWallet(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Unblock(playerId.Value, currency.Value, ct);
            Console.WriteLine("Wallet unblocked.");
        });
    }

    public async Task UpdateWalletBalance(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var newBalance = Prompts.PromptAmount("New balance");
        if (newBalance is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.UpdateBalance(playerId.Value, currency.Value, newBalance.Value, ct);
            Console.WriteLine("Balance updated.");
        });
    }

    public async Task ApplyFundsStrategy(CancellationToken ct = default)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var operation = Prompts.PromptFundsOperation();
        if (operation is null)
            return;

        var amount = Prompts.PromptAmount("Amount");
        if (amount is null)
            return;

        var strategy = _fundsStrategies[operation.Value];

        await RunWalletOperation(async () =>
        {
            var wallet = await _walletRepository.GetWallet(playerId.Value, currency.Value, ct);
            strategy.Execute(wallet, amount.Value);
            await _walletRepository.Save(ct);
            _logger.LogInformation("Applied {Strategy} of {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})",
                strategy.GetType().Name, amount, playerId, currency, wallet.Balance);
            Console.WriteLine($"{operation} operation applied.");
        });
    }

    private async Task RunWalletOperation(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (WalletException ex)
        {
            _logger.LogWarning(ex, "Wallet operation failed");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task<int> GenerateWalletId(CancellationToken ct = default)
    {
        var existingIds = (await _walletRepository.GetAll(ct)).Select(w => w.Id).ToHashSet();

        int id;
        do
        {
            id = Random.Shared.Next(1, int.MaxValue);
        }
        while (existingIds.Contains(id));

        return id;
    }

    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(60);
    private static string WalletCacheKey(int id) => $"wallet:{id}";

    public async Task<Wallet> CreateWallet(int playerId, Currency currency, decimal initialBalance, CancellationToken ct = default)
    {
        if (await _playerRepository.FindPlayer(playerId, ct) is null)
            throw new PlayerNotFoundException(playerId);

        var wallet = new Wallet(await GenerateWalletId(ct), playerId, currency, initialBalance);
        await _walletRepository.Add(wallet, ct);

        // Write-through: cache the new wallet immediately.
        _cache.Set(WalletCacheKey(wallet.Id), wallet, CacheTtl);

        return wallet;
    }

    public async Task<Wallet?> GetWalletById(int walletId, CancellationToken ct = default)
    {
        var key = WalletCacheKey(walletId);

        if (_cache.TryGet(key, out Wallet? cached))
            return cached;

        var all = await _walletRepository.GetAll(ct);
        var wallet = all.FirstOrDefault(w => w.Id == walletId);

        if (wallet is not null)
            _cache.Set(key, wallet, CacheTtl);

        return wallet;
    }

    public async Task<Wallet> DepositAsync(int walletId, decimal amount, CancellationToken ct = default)
    {
        var wallet = (await GetWalletById(walletId, ct))
            ?? throw new WalletNotFoundException(walletId, default);

        wallet.Deposit(amount);
        await _walletRepository.Save(ct);

        // Write-through: refresh the cache with the updated balance.
        _cache.Set(WalletCacheKey(walletId), wallet, CacheTtl);

        return wallet;
    }

    public async Task<Wallet> BlockAsync(int walletId, CancellationToken ct = default)
    {
        var wallet = (await GetWalletById(walletId, ct))
            ?? throw new WalletNotFoundException(walletId, default);

        wallet.Block();
        await _walletRepository.Save(ct);

        _cache.Set(WalletCacheKey(walletId), wallet, CacheTtl);

        return wallet;
    }
    /*private readonly IWalletRepository _walletRepository;
	private readonly IPlayerRepository _playerRepository;
	private readonly ILogger<WalletService> _logger;
	private readonly IReadOnlyDictionary<FundsOperation, IFundsStrategy> _fundsStrategies;

	public WalletService(
		IWalletRepository walletRepository,
		IPlayerRepository playerRepository,
		IEnumerable<IFundsStrategy> strategies,
		ILogger<WalletService> logger)
	{
		_walletRepository = walletRepository;
		_playerRepository = playerRepository;
		_logger = logger;

		// Index every registered strategy by the operation it implements.
		_fundsStrategies = strategies.ToDictionary(strategy => strategy.Operation);
	}

	public void AddWalletToPlayer()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var balance = Prompts.PromptAmount("Initial balance");
		if (balance is null)
			return;

		try
		{
			if (_playerRepository.FindPlayer(playerId.Value) is null)
				throw new PlayerNotFoundException(playerId.Value);

			var wallet = new Wallet(GenerateWalletId(),playerId.Value, currency.Value, balance.Value);
			_walletRepository.Add(wallet);
			Console.WriteLine("Wallet added successfully.");
		}
		catch (PlayerNotFoundException ex)
		{
			_logger.LogWarning(ex, "Could not add wallet, player {PlayerId} not found", playerId);
			Console.WriteLine($"Error: {ex.Message}");
		}
		catch (WalletException ex)
		{
			_logger.LogWarning(ex, "Could not add wallet for player {PlayerId} in {Currency}", playerId, currency);
			Console.WriteLine($"Error: {ex.Message}");
		}
	}

	public void GetWalletsOfPlayer()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var wallets = _walletRepository.GetAllWalletsByPlayerId(playerId.Value);

		if (wallets.Count == 0)
		{
			Console.WriteLine("No wallets found for this player.");
			return;
		}

		foreach (var wallet in wallets)
			Console.WriteLine($"Wallet Number {wallets.IndexOf(wallet)} {wallet}");
	}

	public void DepositToWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var amount = Prompts.PromptAmount("Amount to deposit");
		if (amount is null)
			return;

		RunWalletOperation(() =>
		{
			_walletRepository.Deposit(playerId.Value, currency.Value, amount.Value);
			Console.WriteLine("Deposit successful.");
		});
	}

	public void WithdrawFromWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var amount = Prompts.PromptAmount("Amount to withdraw");
		if (amount is null)
			return;

		RunWalletOperation(() =>
		{
			_walletRepository.Withdraw(playerId.Value, currency.Value, amount.Value);
			Console.WriteLine("Withdrawal successful.");
		});
	}

	public void BlockWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		RunWalletOperation(() =>
		{
			_walletRepository.Block(playerId.Value, currency.Value);
			Console.WriteLine("Wallet blocked.");
		});
	}

	public void UnblockWallet()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		RunWalletOperation(() =>
		{
			_walletRepository.Unblock(playerId.Value, currency.Value);
			Console.WriteLine("Wallet unblocked.");
		});
	}

	public void UpdateWalletBalance()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var newBalance = Prompts.PromptAmount("New balance");
		if (newBalance is null)
			return;

		RunWalletOperation(() =>
		{
			_walletRepository.UpdateBalance(playerId.Value, currency.Value, newBalance.Value);
			Console.WriteLine("Balance updated.");
		});
	}

	public void ApplyFundsStrategy()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var currency = Prompts.PromptCurrency();
		if (currency is null)
			return;

		var operation = Prompts.PromptFundsOperation();
		if (operation is null)
			return;

		var amount = Prompts.PromptAmount("Amount");
		if (amount is null)
			return;

		// Pick the strategy that matches the chosen operation (resolved from DI, no factory).
		var strategy = _fundsStrategies[operation.Value];

		RunWalletOperation(() =>
		{
			var wallet = _walletRepository.GetWallet(playerId.Value, currency.Value);
			strategy.Execute(wallet, amount.Value);
            _walletRepository.Save(); //save method call
            _logger.LogInformation("Applied {Strategy} of {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})",
				strategy.GetType().Name, amount, playerId, currency, wallet.Balance);
			Console.WriteLine($"{operation} operation applied.");
		});
	}

	// Runs a wallet operation and turns any domain (WalletException) failure into a friendly message + log.
	private void RunWalletOperation(Action operation)
	{
		try
		{
			operation();
		}
		catch (WalletException ex)
		{
			_logger.LogWarning(ex, "Wallet operation failed");
			Console.WriteLine($"Error: {ex.Message}");
		}
	}
    private int GenerateWalletId()
    {
        var existingIds = _walletRepository.GetAll().Select(p => p.Id).ToHashSet();

        int id;
        do
        {
            id = Random.Shared.Next(1, int.MaxValue);
        }
        while (existingIds.Contains(id));

        return id;
    }
	*/
}
