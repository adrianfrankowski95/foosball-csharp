using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class FinishedSets : Sets
{
    public IReadOnlyList<FinishedSet> Get() => _sets.Cast<FinishedSet>().ToList();

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

    private void AddSets(IEnumerable<FinishedSet> sets)
    {
        _sets.AddRange(sets);
    }

    public static FinishedSets Finish(SetsInProgress sets)
        => new(sets);
}
