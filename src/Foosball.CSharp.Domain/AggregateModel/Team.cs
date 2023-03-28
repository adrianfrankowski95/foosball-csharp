using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public abstract class Team : Entity<TeamId>
{
    public NonEmptyString Name { get; }

    protected Team(NonEmptyString name)
    {
        if (name is null)
        {
            throw new FoosballDomainException("The name of the team is required.");
        }

        Id = TeamId.Create();
        Name = name;
    }
}

public sealed class TeamId : ValueObject
{
    public Guid Value { get; }

    private TeamId(Guid value)
    {
        Value = value;
    }

    public static TeamId Create()
        => new(Guid.NewGuid());

    public static TeamId FromExisting(Guid value)
        => new(value);

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }
}
