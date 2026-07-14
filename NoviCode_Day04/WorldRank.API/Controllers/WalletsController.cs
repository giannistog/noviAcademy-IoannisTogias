using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.Dtos;
using WorldRank.Application.Services;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Api.Controllers;

[ApiController]
[Route("wallets")]
public class WalletsController : ControllerBase
{
    private readonly WalletService _walletService;

    public WalletsController(WalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request, CancellationToken ct)
    {
        try
        {
            var wallet = await _walletService.CreateWallet(request.PlayerId, request.Currency, request.InitialBalance, ct);
            var response = WalletResponse.FromDomain(wallet);

            return CreatedAtAction(nameof(GetWalletById), new { id = response.Id }, response);
        }
        catch (PlayerNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (WalletException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletById(int id, CancellationToken ct)
    {
        var wallet = await _walletService.GetWalletById(id, ct);

        if (wallet is null)
            return NotFound();

        return Ok(WalletResponse.FromDomain(wallet));
    }

    [HttpPost("{id:int}/deposit")]
    public async Task<IActionResult> Deposit(int id, [FromBody] DepositRequest request, CancellationToken ct)
    {
        try
        {
            var wallet = await _walletService.DepositAsync(id, request.Amount, ct);
            return Ok(WalletResponse.FromDomain(wallet));
        }
        catch (WalletNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (WalletException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}