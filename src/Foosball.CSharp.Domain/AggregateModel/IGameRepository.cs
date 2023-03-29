
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public interface IGameRepository : IRepository<Game>
{
    public Task<Game?> GetGameAsync(GameId gameId, CancellationToken cancellationToken = default);
    public Task<bool> AddGameAsync(Game game, CancellationToken cancellationToken = default);
    public Task<bool> UpdateGameAsync(Game updatedGame, CancellationToken cancellationToken = default);
}
