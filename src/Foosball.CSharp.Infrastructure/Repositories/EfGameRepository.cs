using Foosball.CSharp.Domain.GameAggregateModel;
using Microsoft.EntityFrameworkCore;

namespace Foosball.CSharp.Infrastructure.Repositories;

public class EfGameRepository : IGameRepository
{
    private readonly FoosballDbContext _ctx;
    private readonly DbSet<Game> _games;

    public EfGameRepository(FoosballDbContext ctx)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        _games = ctx.Games ?? throw new ArgumentNullException(nameof(ctx));
    }

    public async Task<bool> AddGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        _games.Add(game);
        return (await _ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false)) > 0;
    }

    public Task<Game?> GetGameAsync(GameId gameId, CancellationToken cancellationToken = default)
        => _games.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(gameId), cancellationToken);

    public async Task<bool> UpdateGameAsync(Game updatedGame, CancellationToken cancellationToken = default)
    {
        var updated = false;
        using (var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken))
        {
            // Get existing game from db
            var existingGame = await _games.FindAsync(new GameId[] { updatedGame.Id }, cancellationToken: cancellationToken).ConfigureAwait(false);

            // Rollback whole transaction if game does not exist
            if (existingGame is null)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                throw new InvalidOperationException($"Could not update game with ID: {updatedGame.Id} - game does not exist.");
            }

            // Remove game from db
            _games.Remove(existingGame);
            await _ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Add updated game to db
            _games.Add(updatedGame);
            updated = (await _ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false)) > 0;

            // Commit transaction as an atomic operation
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }

        return updated;
    }
}
