using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class SetsInProgress : Sets
{
    public IReadOnlyList<Set> Get() => _sets;
    private SetsInProgress(GameId gameId, TeamId teamAId, TeamId teamBId)
    {
        BeginNew(SetInProgress.Start(gameId, teamAId, teamBId));
    }

    public static SetsInProgress Begin(GameId gameId, TeamId teamAId, TeamId teamBId)
        => new(gameId, teamAId, teamBId);

    public Sets UpdateCurrent(Scores scores)
    {
        var lastSet = _sets[^1];
        if (lastSet is not SetInProgress setInProgress)
        {
            throw new FoosballDomainException("There are no sets in progress that could be updated.");
        }

        lastSet = setInProgress.UpdateScores(scores);

        if (!AllFinished())
        {
            BeginNew(SetInProgress.Start(lastSet.GameId, lastSet.TeamAId, lastSet.TeamBId));
            return this;
        }

        return FinishedSets.Finish(this);
    }

    private void BeginNew(SetInProgress newSet)
    {
        _sets.Add(newSet);
    }

    private Dictionary<TeamId, int> GetWins()
        => _sets
            .Where(s => s is FinishedSet)
            .Cast<FinishedSet>()
            .GroupBy(s => s.WinnerTeamId)
            .ToDictionary(s => s.Key, s => s.Count());

    private bool AllFinished()
    {
        var maxCountReached = _sets.Count == MaxCount && _sets[^1] is FinishedSet;
        var maxWinsReached = GetWins().Any(w => w.Value == RequiredToWin);

        return maxCountReached || maxWinsReached;
    }
}
