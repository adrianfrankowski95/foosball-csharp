using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.TeamAggregateModel;


// Note: in this example Teams do not change their state, but are still modeled as an aggregate root
// In a real-world scenario, teams could have their players replaced or points scored after a won game
public abstract class Team : Entity<TeamId>, IAggregateRoot
{
    public NonEmptyString Name { get; }

    // Required by EF :-( Protected, exposed to derived types :-|
    protected Team() { }

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
