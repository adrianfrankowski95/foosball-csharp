using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class FinishedSets : Sets
{
    private FinishedSets(SetsInProgress sets)
    {
        if (sets is null)
        {
            throw new FoosballDomainException("Sets cannot be null.");
        }

        if (sets.Get().Any(s => s is not FinishedSet))
        {
            throw new FoosballDomainException("Finished sets cannot contain sets that are still in progress.");
        }

        AddSets(sets.Get().Cast<FinishedSet>());
    }

    public static FinishedSets Finish(SetsInProgress sets)
        => new(sets);

    private Dictionary<TeamId, int> GetWins()
        => _sets.Cast<FinishedSet>()
            .GroupBy(s => s.WinnerTeamId)
            .ToDictionary(s => s.Key, s => s.Count());

    public TeamId GetWinner() => GetWins().First(w => w.Value == GetWins().Values.Max()).Key;

    private void AddSets(IEnumerable<FinishedSet> sets)
    {
        _sets.AddRange(sets);
    }
}
