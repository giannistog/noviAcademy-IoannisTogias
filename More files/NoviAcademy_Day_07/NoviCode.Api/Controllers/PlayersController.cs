using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoviCode.Commands.Players;

namespace NoviCode.Api;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
	private readonly IPlayerService _players;
	private readonly IMediator _mediator;

	public PlayersController(IPlayerService players, IMediator mediator)
	{
        _players = players;
		_mediator = mediator;
    }

	// POST /players — create a player (the decorator writes through to the cache).
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request, CancellationToken cancellationToken)
	{
		Guid id;
		try
		{
			id = await _mediator.Send(new CreatePlayerCommand(request.Name, request.Score));
		}
		catch (ArgumentException ex)
		{
			// Empty name / negative score → 400.
			return BadRequest(new { error = ex.Message });
		}

		return CreatedAtAction(nameof(GetById), new { id = id });
	}

	// GET /players/{id} — 200 or 404 (caching handled by the decorator).
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var player = await _players.GetByIdAsync(id, cancellationToken);
		return player is null ? NotFound() : Ok(PlayerResponse.From(player));
	}

	// GET /players — list all players (cached).
	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var players = await _players.GetAllAsync(cancellationToken);
		return Ok(players.Select(PlayerResponse.From));
	}
}
