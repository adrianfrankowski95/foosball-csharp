
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public abstract class Set : Entity<SetId>
{
    public GameId GameId { get; protected set; }
    public TeamId TeamAId { get; protected set; }
    public TeamId TeamBId { get; protected set; }
    public Scores Scores { get; protected set; }

    protected Set(GameId gameId, TeamId teamAId, TeamId teamBId)
    {
        if (gameId is null)
        {
            throw new FoosballDomainException("Set must have its game assigned.");
        }

        if (teamAId is null)
        {
            throw new FoosballDomainException("Set requires a team A to be assigned.");
        }

        if (teamBId is null)
        {
            throw new FoosballDomainException("Set requires a team B to be assigned.");
        }

        if (teamAId.Equals(teamBId))
        {
            throw new FoosballDomainException("Teams must be different to start a set.");
        }

        GameId = gameId;
        TeamAId = teamAId;
        TeamBId = teamBId;
    }
}

public sealed class SetId
{
    public Guid Value { get; }

    private SetId(Guid value)
    {
        Value = value;
    }

    public static SetId Create()
        => new(Guid.NewGuid());

    public static SetId FromExisting(Guid value)
        => new(value);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override bool Equals(object? obj)
    {
        return Value.Equals(obj);
    }
}