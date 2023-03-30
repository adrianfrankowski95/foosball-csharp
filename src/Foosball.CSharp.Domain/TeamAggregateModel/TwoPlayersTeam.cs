using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.TeamAggregateModel;

public class TwoPlayerTeam : Team
{
    public PlayerId FirstPlayerId { get; }
    public PlayerId SecondPlayerId { get; }

    // Required by EF :-( Private, though :-)
    private TwoPlayerTeam() : base() { }

    public TwoPlayerTeam(NonEmptyString teamName, PlayerId firstPlayerId, PlayerId secondPlayerId) : base(teamName)
    {
        if (firstPlayerId is null || secondPlayerId is null)
        {
            throw new FoosballDomainException("Two-players team must have two players.");
        }

        FirstPlayerId = firstPlayerId;
        SecondPlayerId = secondPlayerId;
    }
}
