using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public abstract class Game : Entity<GameId>, IAggregateRoot
{
    public Sets Sets { get; protected set; }
    public DateTime StartedAt { get; protected set; }
}

public sealed class GameId : ValueObject
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

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return Value;
    }
}