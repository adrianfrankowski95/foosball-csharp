using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class Sets
{
    public const int MaxCount = 3;
    public const int RequiredToWin = 2;
    private readonly List<Set> _sets;

    private Sets(GameId gameId, TeamId teamAId, TeamId teamBId)
    {
        _sets = new()
        {
            SetInProgress.Start(gameId, teamAId, teamBId)
        };
    }

    public static Sets Begin(GameId gameId, TeamId teamAId, TeamId teamBId)
        => new(gameId, teamAId, teamBId);

    public void UpdateCurrent(SetResult newResult)
    {
        var lastSet = _sets[^1];
        if (lastSet is not SetInProgress inProgress)
        {
            throw new FoosballDomainException("There are no sets in progress that could be updated.");
        }

        lastSet = inProgress.UpdateResult(newResult);

        if (!AreFinished())
        {
            BeginNew(SetInProgress.Start(lastSet.GameId, lastSet.TeamAId, lastSet.TeamBId));
        }
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

    public bool AreFinished()
    {
        var maxCountReached = _sets.Count == MaxCount && _sets[^1] is FinishedSet;
        var maxWinsReached = GetWins().Any(w => w.Value == RequiredToWin);

        return maxCountReached || maxWinsReached;
    }

    public TeamId GetWinner() =>
        AreFinished()
        ? GetWins().First(v => v.Value == GetWins().Values.Max()).Key
        : throw new FoosballDomainException("Could not get a winner, because sets have not finished yet.");
}
