using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public abstract class Game : Entity<GameId>, IAggregateRoot
{
    public Sets Sets { get; protected set; }
    public DateTime StartedAt { get; protected set; }
}

public sealed class GameId
{
    public Guid Value { get; }

    private GameId(Guid value)
    {
        Value = value;
    }

    public static GameId Create()
        => new(Guid.NewGuid());

    public static GameId FromExisting(Guid value)
        => new(value);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override bool Equals(object? obj)
    {
        return Value.Equals(obj);
    }
}