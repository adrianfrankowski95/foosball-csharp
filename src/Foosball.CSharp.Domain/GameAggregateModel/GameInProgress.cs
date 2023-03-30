
using Foosball.CSharp.Domain.Events;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Domain.GameAggregateModel;

public class GameInProgress : Game
{
    public const int MaxSets = 3;
    public const int SetsRequiredToWin = 2;
    private readonly List<Set> _sets;
    public IReadOnlyList<Set> Sets => _sets;

    // Required by EF :-( Private, though :-)
    private GameInProgress() { }

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

        _sets = new();
        StartNewSet(SetInProgress.Start(Id, teamAId, teamBId));
        StartedAt = startedAt;

        AddDomainEvent(new GameCreatedDomainEvent(Id, teamAId, teamBId, startedAt));
    }

    // According to the Foosball rules, teams may contain either 1 or 2 players
    public static GameInProgress Create(TwoPlayerTeam teamA, TwoPlayerTeam teamB, DateTime startedAt)
        => new(teamA.Id, teamB.Id, startedAt);

    public static GameInProgress Create(OnePlayerTeam teamA, OnePlayerTeam teamB, DateTime startedAt)
        => new(teamA.Id, teamB.Id, startedAt);

    public Game UpdateOrBeginSet(Scores scores, DateTime now)
    {
        var lastSet = _sets[^1];

        if (lastSet is FinishedSet)
        {
            if (_sets.Count == MaxSets)
            {
                throw new FoosballDomainException($"There are no sets in progress that could be updated and maximum number of sets ({MaxSets}) has been reached.");
            }

            BeginSetWithScores(SetInProgress.WithScores(lastSet.GameId, lastSet.TeamAId, lastSet.TeamBId, scores, now));
        }

        if (lastSet is SetInProgress setInProgress)
        {
            UpdateLastSet(setInProgress.UpdateScores(scores, now));
        }

        if (GameFinished())
        {
            return FinishedGame.Finish(this);
        }

        AddDomainEvent(new GameUpdatedDomainEvent(Id, lastSet.TeamAId, lastSet.TeamBId, Sets));

        return this;
    }

    private void StartNewSet(SetInProgress newSet)
    {
        _sets.Add(newSet);
    }

    private void BeginSetWithScores(Set newSet)
    {
        _sets.Add(newSet);
    }

    private void UpdateLastSet(Set updatedSet)
    {
        _sets[^1] = updatedSet;
    }

    private bool GameFinished()
    {
        var maxSetsReached = _sets.Count == MaxSets && _sets[^1] is FinishedSet;
        var maxWinsReached = GetWins().Any(w => w.Value == SetsRequiredToWin);

        return maxSetsReached || maxWinsReached;
    }

    private Dictionary<TeamId, int> GetWins()
        => _sets
            .Where(s => s is FinishedSet)
            .Cast<FinishedSet>()
            .GroupBy(s => s.WinnerTeamId)
            .ToDictionary(s => s.Key, s => s.Count());
}
