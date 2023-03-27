using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class TwoPlayersTeam : Team
{
    public PlayerId FirstPlayerId { get; }
    public PlayerId SecondPlayerId { get; }

    public TwoPlayersTeam(NonEmptyString name, PlayerId firstPlayerId, PlayerId secondPlayerId) : base(name)
    {
        if (firstPlayerId is null)
        {
            throw new FoosballDomainException("Two-players team must have two players.");
        }

        if (secondPlayerId is null)
        {
            throw new FoosballDomainException("Two-players team must have two players.");
        }

        FirstPlayerId = firstPlayerId;
        SecondPlayerId = secondPlayerId;
    }
}
