
using Foosball.CSharp.Domain.Events;
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

        AddDomainEvent(new GameCreatedDomainEvent(Id, teamAId, teamBId, startedAt));
    }

    // According to the Foosball rules, team may contain either 1 or 2 players
    public static GameInProgress Create(TwoPlayersTeam teamA, TwoPlayersTeam teamB, DateTime startedAt)
        => new(teamA.Id, teamB.Id, startedAt);

    public static GameInProgress Create(OnePlayerTeam teamA, OnePlayerTeam teamB, DateTime startedAt)
        => new(teamA.Id, teamB.Id, startedAt);

    public Game UpdateCurrentSet(SetResult newResult)
    {
        if (Sets is not SetsInProgress setsInProgress)
        {
            throw new FoosballDomainException("Game in progress must contain sets that are still in progress.");
        }

        Sets = setsInProgress.UpdateCurrent(newResult);

        if (Sets is FinishedSets)
        {
            return FinishedGame.Finish(this);
        }

        var lastSet = Sets.Last();
        AddDomainEvent(new GameUpdatedDomainEvent(Id, lastSet.TeamAId, lastSet.TeamBId, setsInProgress));

        return this;
    }
}
