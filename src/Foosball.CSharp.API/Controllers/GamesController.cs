using System.ComponentModel.DataAnnotations;
using Foosball.CSharp.API.Application.Commands;
using Foosball.CSharp.API.Application.Queries;
using Foosball.CSharp.API.Application.Queries.Models;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Microsoft.AspNetCore.Mvc;

namespace Foosball.CSharp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameRepository _gameRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IGameQueries _gameQueries;

    public GamesController(IGameRepository games, ITeamRepository teams, IGameQueries gameQueries)
    {
        _gameRepository = games ?? throw new ArgumentNullException(nameof(games));
        _teamRepository = teams ?? throw new ArgumentNullException(nameof(teams));
        _gameQueries = gameQueries ?? throw new ArgumentNullException(nameof(gameQueries));
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
        var teamA = await _teamRepository.GetTeamAsync(TeamId.FromExisting(command.TeamAId)).ConfigureAwait(false);
        var teamB = await _teamRepository.GetTeamAsync(TeamId.FromExisting(command.TeamBId)).ConfigureAwait(false);

        if (teamA is null || teamB is null)
        {
            return NotFound("Could not find provided teams to start a game.");
        }

        GameInProgress? game;
        try
        {
            switch (teamA)
            {
                case OnePlayerTeam onePlayerTeamA when teamB is OnePlayerTeam onePlayerTeamB:
                    game = GameInProgress.Create(onePlayerTeamA, onePlayerTeamB, DateTime.UtcNow);
                    break;

                case TwoPlayersTeam twoPlayersTeamA when teamB is TwoPlayersTeam twoPlayersTeamB:
                    game = GameInProgress.Create(twoPlayersTeamA, twoPlayersTeamB, DateTime.UtcNow);
                    break;

                default:
                    return BadRequest("Could not create game with provided teams.");
            };
        }
        catch (FoosballDomainException ex)
        {
            return BadRequest(ex.Message);
        }

        var success = await _gameRepository.AddGameAsync(game).ConfigureAwait(false);

        return success
            ? Ok($"Successfully started game with ID: {game.Id.Value}.")
            : Problem("Error starting a new game.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGameAsync([FromBody, Required] UpdateGameCommand command)
    {
        var game = await _gameRepository.GetGameAsync(GameId.FromExisting(command.GameId)).ConfigureAwait(false);

        if (game is null)
        {
            return NotFound($"Could not find a game with provided ID: {command.GameId}");
        }

        if (game is not GameInProgress gameInProgress)
        {
            return BadRequest("Could not update a game that is not in progress anymore.");
        }

        Game? updatedGame;
        try
        {
            updatedGame = gameInProgress.UpdateCurrentSet(
                Scores.Set(command.TeamAScore.Goals(), command.TeamBScore.Goals()),
                DateTime.UtcNow);
        }
        catch (FoosballDomainException ex)
        {
            return BadRequest(ex.Message);
        }

        var success = await _gameRepository.UpdateGameAsync(updatedGame).ConfigureAwait(false);

        return success
            ? Ok(@$"Successfully updated game with ID {game.Id.Value}. {(updatedGame is FinishedGame finished
                    ? $"Game has finished, winner: {finished.WinnerTeamId.Value}."
                    : "The game is still in progress.")}")

            : Problem("Error updating a new game.");
    }
}
