using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.Dtos;
using WorldRank.Application.Services;

namespace WorldRank.Api.Controllers;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
    private readonly PlayerService _playerService;

    public PlayersController(PlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request, CancellationToken ct)
    {
        try
        {
            var player = await _playerService.CreatePlayer(request.Name, request.Score, ct);
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
        var player = await _playerService.GetPlayerById(id, ct);

        if (player is null)
            return NotFound();

        return Ok(PlayerResponse.FromDomain(player));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlayers(CancellationToken ct)
    {
        var players = await _playerService.GetAllPlayersAsync(ct);
        var response = players.Select(PlayerResponse.FromDomain);

        return Ok(response);
    }
}