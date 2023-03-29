using Foosball.CSharp.API.Application.Queries.Models;
using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.API.Application.Queries;

public interface IGameQueries
{
    public Task<GameDetails?> GetGameDetailsAsync(GameId gameId);
    public IAsyncEnumerable<GameOverview> GetGameOverviewsAsync();
}
