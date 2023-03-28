using Foosball.CSharp.Domain.Events;
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

        if (game.Sets is not FinishedSets finishedSets)
        {
            throw new FoosballDomainException("All sets must be finished to consider game as finished.");
        }

        Id = game.Id;
        Sets = finishedSets;
        WinnerTeamId = finishedSets.GetWinner();

        var lastSet = finishedSets.Last();

        AddDomainEvent(new GameFinishedDomainEvent(Id, lastSet.TeamAId, lastSet.TeamBId, WinnerTeamId, finishedSets));
    }

    public static FinishedGame Finish(GameInProgress game)
        => new(game);
}
