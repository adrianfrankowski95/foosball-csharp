using System.ComponentModel.DataAnnotations;
using Foosball.CSharp.API.Application.Commands;
using Foosball.CSharp.API.Application.Queries;
using Foosball.CSharp.API.Application.Queries.Models;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foosball.CSharp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameQueries _gameQueries;
    private readonly ICreateGameCommandHandler _createGameCommandHandler;
    private readonly IUpdateGameCommandHandler _updateGameCommandHandler;

    public GamesController(
        IGameQueries gameQueries,
        ICreateGameCommandHandler createGameCommandHandler,
        IUpdateGameCommandHandler updateGameCommandHandler)
    {
        _gameQueries = gameQueries ?? throw new ArgumentNullException(nameof(gameQueries));
        _createGameCommandHandler = createGameCommandHandler ?? throw new ArgumentNullException(nameof(createGameCommandHandler));
        _updateGameCommandHandler = updateGameCommandHandler ?? throw new ArgumentNullException(nameof(updateGameCommandHandler));
    }

    [HttpGet]
    public ActionResult<IAsyncEnumerable<GameOverview>> GetGameOverviews()
    {
        return Ok(_gameQueries.GetGameOverviewsAsync());
    }

    [HttpGet("{id:guid:required}")]
    public async Task<ActionResult<GameDetails>> GetGameDetails([FromRoute, Required] Guid id)
    {
        var gameDetails = await _gameQueries.GetGameDetailsAsync(GameId.FromExisting(id)).ConfigureAwait(false);

        return gameDetails is null
            ? NotFound($"Could not find details of the game with ID: {id}")
            : Ok(gameDetails);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGameAsync([FromBody, Required] CreateGameCommand command)
    {
        GameId? newGameId;
        try
        {
            newGameId = await _createGameCommandHandler.HandleAsync(command).ConfigureAwait(false);
        }
        catch (FoosballDomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            return Problem(ex.Message);
        }

        return Ok($"Successfully started game with ID: {newGameId.Value}.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGameAsync([FromBody, Required] UpdateGameCommand command)
    {
        bool success;
        try
        {
            success = await _updateGameCommandHandler.HandleAsync(command).ConfigureAwait(false);
        }
        catch (FoosballDomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            return Problem(ex.Message);
        }

        return success
            ? Ok($"Successfully updated game with ID {command.GameId}.")
            : Problem($"Error updating game with ID {command.GameId}.");
    }
}
