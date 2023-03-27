

using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class FinishedGame : Game
{
    public TeamId WinnerTeamId { get; }

    private FinishedGame(GameInProgress game)
    {
        if (game is null)
        {
            throw new FoosballDomainException("Game cannot be null.");
        }

        if (!game.Sets.AreFinished())
        {
            throw new FoosballDomainException($"All sets must be finished to consider game as finished.");
        }

        var winner = game.Sets.GetWinner();
        if (winner is null)
        {
            throw new FoosballDomainException($"Winner must not be null in finished game.");
        }

        Id = game.Id;
        Sets = game.Sets;
        WinnerTeamId = game.Sets.GetWinner();
    }

    public static FinishedGame Finish(GameInProgress game)
        => new(game);
}
