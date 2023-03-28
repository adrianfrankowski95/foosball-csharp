using System.Collections;

namespace Foosball.CSharp.Domain.AggregateModel;

public abstract class Sets : IEnumerable<Set>
{
    public const int MaxCount = 3;
    public const int RequiredToWin = 2;
    protected readonly List<Set> _sets = new();

    public IEnumerator<Set> GetEnumerator()
        => _sets.GetEnumerator();


    IEnumerator IEnumerable.GetEnumerator()
        => _sets.GetEnumerator();
}
