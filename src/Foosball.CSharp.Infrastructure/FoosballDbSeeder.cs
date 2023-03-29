
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Foosball.CSharp.Infrastructure;

public class FoosballDbSeeder : IHostedService
{
    private readonly IServiceProvider _services;

    public FoosballDbSeeder(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<FoosballDbContext>();

        if (!ctx.Games.Any())
        {
            var players = GetPlayers();
            var teams = GetTeams(players);
            var game = GetGame(teams);

            ctx.AddRange(players.firstPlayer, players.secondPlayer);
            ctx.AddRange(teams.firstTeam, teams.secondTeam);
            ctx.Add(game);
        }

        await ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static (Player firstPlayer, Player secondPlayer) GetPlayers()
        => (new Player("Cristiano", "Ronaldo"), new Player("Lionel", "Messi"));

    private static (OnePlayerTeam firstTeam, OnePlayerTeam secondTeam) GetTeams((Player firstPlayer, Player secondPlayer) playersPair)
        => (new OnePlayerTeam($"Team {playersPair.firstPlayer.FirstName} {playersPair.firstPlayer.LastName} {DateTime.UtcNow.Ticks}", playersPair.firstPlayer.Id),
            new OnePlayerTeam($"Team {playersPair.secondPlayer.FirstName} {playersPair.secondPlayer.LastName} {DateTime.UtcNow.Ticks}", playersPair.secondPlayer.Id));

    private static Game GetGame((OnePlayerTeam firstTeam, OnePlayerTeam secondTeam) teamsPair)
        => GameInProgress.Create(teamsPair.firstTeam, teamsPair.secondTeam, DateTime.UtcNow);
}
