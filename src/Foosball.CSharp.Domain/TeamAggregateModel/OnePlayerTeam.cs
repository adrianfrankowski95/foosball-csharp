using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.TeamAggregateModel;

public class OnePlayerTeam : Team
{
    public PlayerId PlayerId { get; }

    // Required by EF :-( Private, though :-)
    private OnePlayerTeam() : base() { }

    public OnePlayerTeam(NonEmptyString teamName, PlayerId playerId) : base(teamName)
    {
        if (playerId is null)
        {
            throw new FoosballDomainException("One-player team must have one player.");
        }

        PlayerId = playerId;
    }
}
