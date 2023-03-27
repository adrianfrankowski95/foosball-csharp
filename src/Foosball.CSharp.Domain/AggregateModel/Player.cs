
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public class Player : Entity<PlayerId>
{
    public NonEmptyString FirstName { get; }
    public NonEmptyString LastName { get; }

    public Player(NonEmptyString firstName, NonEmptyString lastName)
    {
        if (firstName is null)
        {
            throw new FoosballDomainException("First name of the player is required.");
        }

        if (lastName is null)
        {
            throw new FoosballDomainException("Last name of the player is required.");
        }

        Id = PlayerId.Create();
        FirstName = firstName;
        LastName = lastName;
    }
}

public sealed class PlayerId
{
    public Guid Value { get; }

    private PlayerId(Guid value)
    {
        Value = value;
    }

    public static PlayerId Create()
        => new(Guid.NewGuid());

    public static PlayerId FromExisting(Guid value)
        => new(value);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override bool Equals(object? obj)
    {
        return Value.Equals(obj);
    }
}