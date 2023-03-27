
using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class GameInProgress : Game
{
    private GameInProgress(TeamId teamAId, TeamId teamBId, DateTime startedAt) : base()
    {
        if (teamAId is null || teamBId is null)
        {
            throw new FoosballDomainException("Both teams are required to start a game.");
        }

        if (teamAId.Equals(teamBId))
        {
            throw new FoosballDomainException("Teams must be different to start a game.");
        }

        Id = GameId.Create();
        Sets = SetsInProgress.Begin(Id, teamAId, teamBId);
        StartedAt = startedAt;
    }

    // According to the Foosball rules, team may contain either 1 or 2 players
    public static GameInProgress Start(TwoPlayersTeam teamA, TwoPlayersTeam teamB, DateTime startedAt)
        => new(teamA.Id, teamB.Id, startedAt);

    public static GameInProgress Start(OnePlayerTeam teamA, OnePlayerTeam teamB, DateTime startedAt)
        => new(teamA.Id, teamB.Id, startedAt);

    public Game UpdateCurrentSet(SetResult newResult)
    {
        if (Sets is not SetsInProgress setsInProgress)
        {
            throw new FoosballDomainException("Game in progress must contain sets that are still in progress.");
        }

        Sets = setsInProgress.UpdateCurrent(newResult);

        return Sets is FinishedSets ? FinishedGame.Finish(this) : this;
    }
}
