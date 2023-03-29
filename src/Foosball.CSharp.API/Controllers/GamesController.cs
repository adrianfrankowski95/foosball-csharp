using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.AspNetCore.Mvc;

namespace Foosball.CSharp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly IGameRepository _games;

    public GamesController(ILogger<GamesController> logger, IGameRepository games)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _games = games ?? throw new ArgumentNullException(nameof(games));
    }

    [HttpGet(Name = "GetGames")]
    public async Task<IActionResult> Get()
    {
        var guid = Guid.Parse("75449e73-bd0c-4f4f-a029-f89af8bc9868");
        var game = await _games.GetGameAsync(GameId.FromExisting(guid)).ConfigureAwait(false);

        if (game is GameInProgress gameInProgress)
        {
            var newGame = gameInProgress.UpdateCurrentSet(Scores.Set(8.Goals(), 10.Goals()));
            await _games.UpdateGameAsync(newGame).ConfigureAwait(false);
        }

        return Ok(await _games.GetGameAsync(game.Id).ConfigureAwait(false));
    }
}
