using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public class SetResult : ValueObject
{
    public Goals TeamAGoals { get; }
    public Goals TeamBGoals { get; }

    public SetResult(Goals teamAGoals, Goals teamBGoals)
    {
        if (teamAGoals is null || teamBGoals is null)
        {
            throw new FoosballDomainException("Set results cannot be null.");
        }

        TeamAGoals = teamAGoals;
        TeamBGoals = teamBGoals;
    }

    public bool IsFinished() => TeamAGoals.Value == Goals.MaxValue || TeamBGoals.Value == Goals.MaxValue;
    public bool IsEqual() => TeamAGoals == TeamBGoals;
    public bool AnyLowerThan(SetResult other) => TeamAGoals < other.TeamAGoals || TeamBGoals < other.TeamBGoals;

    public static SetResult Empty() => new(0.Goals(), 0.Goals());

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return TeamAGoals;
        yield return TeamBGoals;
    }
}
