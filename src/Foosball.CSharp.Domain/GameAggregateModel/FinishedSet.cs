
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Domain.GameAggregateModel;

public class FinishedSet : Set
{
    public TeamId WinnerTeamId { get; }
    public DateTime FinishedAt { get; }

    // Required by EF :-( Private, though :-)
    private FinishedSet() : base() { }

    private FinishedSet(SetInProgress set, DateTime finishedAt) : base(set.GameId, set.TeamAId, set.TeamBId)
    {
        if (set is null)
        {
            throw new FoosballDomainException("Set cannot be null.");
        }

        if (!set.Scores.HaveWinner())
        {
            throw new FoosballDomainException("There are not enough goals to consider this set as finished.");
        }

        if (set.Scores.IsDraw())
        {
            throw new FoosballDomainException("There must be an only one winner.");
        }

        Id = set.Id;
        Scores = set.Scores;
        WinnerTeamId = Scores.TeamAScore > Scores.TeamBScore ? TeamAId : TeamBId;
        FinishedAt = finishedAt;
    }

    public static FinishedSet Finish(SetInProgress set, DateTime finishedAt)
        => new(set, finishedAt);
}
