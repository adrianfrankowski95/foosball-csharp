using Microsoft.AspNetCore.Mvc;

namespace Foosball.CSharp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;

    public GamesController(ILogger<GamesController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetGames")]
    public IActionResult Get()
    {
        return Ok();
    }
}
