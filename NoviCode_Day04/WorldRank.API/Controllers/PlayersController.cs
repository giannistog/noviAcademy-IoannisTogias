using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.Dtos;
using WorldRank.Application.Abstractions;
using WorldRank.Application.Commands;
using WorldRank.Application.Queries;
using WorldRank.Domain.Entities;

namespace WorldRank.Api.Controllers;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
    private readonly ICommandHandler<CreatePlayerCommand, Player> _createPlayer;
    private readonly IQueryHandler<GetPlayerByIdQuery, Player?> _getPlayerById;
    private readonly IQueryHandler<GetAllPlayersQuery, IEnumerable<Player>> _getAllPlayers;

    public PlayersController(
        ICommandHandler<CreatePlayerCommand, Player> createPlayer,
        IQueryHandler<GetPlayerByIdQuery, Player?> getPlayerById,
        IQueryHandler<GetAllPlayersQuery, IEnumerable<Player>> getAllPlayers)
    {
        _createPlayer = createPlayer;
        _getPlayerById = getPlayerById;
        _getAllPlayers = getAllPlayers;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request, CancellationToken ct)
    {
        try
        {
            var player = await _createPlayer.Handle(new CreatePlayerCommand(request.Name, request.Score), ct);
            var response = PlayerResponse.FromDomain(player);

            return CreatedAtAction(nameof(GetPlayerById), new { id = response.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPlayerById(int id, CancellationToken ct)
    {
        var player = await _getPlayerById.Handle(new GetPlayerByIdQuery(id), ct);

        if (player is null)
            return NotFound();

        return Ok(PlayerResponse.FromDomain(player));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlayers(CancellationToken ct)
    {
        var players = await _getAllPlayers.Handle(new GetAllPlayersQuery(), ct);
        var response = players.Select(PlayerResponse.FromDomain);

        return Ok(response);
    }
}