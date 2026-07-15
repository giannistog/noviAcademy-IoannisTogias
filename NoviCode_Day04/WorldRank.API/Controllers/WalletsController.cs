using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.Dtos;
using WorldRank.Application.Abstractions;
using WorldRank.Application.Commands;
using WorldRank.Application.Queries;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Api.Controllers;

[ApiController]
[Route("wallets")]
public class WalletsController : ControllerBase
{
    private readonly ICommandHandler<CreateWalletCommand, Wallet> _createWallet;
    private readonly ICommandHandler<DepositCommand, Wallet> _deposit;
    private readonly ICommandHandler<BlockWalletCommand, Wallet> _block;
    private readonly IQueryHandler<GetWalletByIdQuery, Wallet?> _getWalletById;

    public WalletsController(
        ICommandHandler<CreateWalletCommand, Wallet> createWallet,
        ICommandHandler<DepositCommand, Wallet> deposit,
        ICommandHandler<BlockWalletCommand, Wallet> block,
        IQueryHandler<GetWalletByIdQuery, Wallet?> getWalletById)
    {
        _createWallet = createWallet;
        _deposit = deposit;
        _block = block;
        _getWalletById = getWalletById;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request, CancellationToken ct)
    {
        try
        {
            var wallet = await _createWallet.Handle(
                new CreateWalletCommand(request.PlayerId, request.Currency, request.InitialBalance), ct);
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
        var wallet = await _getWalletById.Handle(new GetWalletByIdQuery(id), ct);

        if (wallet is null)
            return NotFound();

        return Ok(WalletResponse.FromDomain(wallet));
    }

    [HttpPost("{id:int}/deposit")]
    public async Task<IActionResult> Deposit(int id, [FromBody] DepositRequest request, CancellationToken ct)
    {
        try
        {
            var wallet = await _deposit.Handle(new DepositCommand(id, request.Amount), ct);
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

    [HttpPost("{id:int}/block")]
    public async Task<IActionResult> Block(int id, CancellationToken ct)
    {
        try
        {
            var wallet = await _block.Handle(new BlockWalletCommand(id), ct);
            return Ok(WalletResponse.FromDomain(wallet));
        }
        catch (WalletNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}