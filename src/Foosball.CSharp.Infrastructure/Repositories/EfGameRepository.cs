using Foosball.CSharp.Domain.AggregateModel;
using Foosball.CSharp.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Foosball.CSharp.Infrastructure.Repositories;

public class EfGameRepository : IGameRepository
{
    private readonly DbSet<Game> _games;
    public IUnitOfWork UnitOfWork { get; }

    public EfGameRepository(FoosballDbContext ctx, IUnitOfWork unitOfWork)
    {
        _games = ctx?.Games ?? throw new ArgumentNullException(nameof(ctx));
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public void AddGame(Game game)
        => _games.Add(game);

    public ValueTask<Game?> FindGameAsync(GameId gameId)
        => _games.FindAsync(gameId);

    public void UpdateGame(Game game)
        => _games.Update(game);
}
