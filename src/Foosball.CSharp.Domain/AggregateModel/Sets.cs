namespace Foosball.CSharp.Domain.AggregateModel;

public class Sets
{
    public const int MaxCount = 3;
    public const int RequiredToWin = 2;
    protected readonly List<Set> _sets = new();
}
