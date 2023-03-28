
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public interface IGameRepository : IRepository<Game>
{
    public IUnitOfWork UnitOfWork { get; }
    public ValueTask<Game?> FindGameAsync(GameId gameId);
    public void AddGame(Game game);
    public void UpdateGame(Game game);
}
