using Microsoft.AspNetCore.Mvc;
using WorldRank.Api.Dtos;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Services;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Repositories;
namespace WorldRank.API.Controllers;

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
        var player = await _playerService.CreatePlayer(request.Name, request.Score, ct);
        var response = PlayerResponse.FromDomain(player);

        return CreatedAtAction(nameof(GetPlayerById), new { id = response.Id }, response);
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
/*namespace WorldRank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayersController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _playerService.GetAllPlayers();

                if (result.Count == 0)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{playerId:int}")]
        public IActionResult GetPlayerById(int playerId)
        {
            try
            {
                var result = _playerService.FindPlayerById(playerId);

                if (result is null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}*/