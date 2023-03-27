using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class OnePlayerTeam : Team
{
    public PlayerId PlayerId { get; }

    public OnePlayerTeam(NonEmptyString name, PlayerId playerId) : base(name)
    {
        if (playerId is null)
        {
            throw new FoosballDomainException("One-player team must have one player.");
        }

        PlayerId = playerId;
    }
}
