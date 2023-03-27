
using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class FinishedSet : Set
{
    public TeamId WinnerTeamId { get; }

    private FinishedSet(SetInProgress set) : base(set.GameId, set.TeamAId, set.TeamBId)
    {
        if (set is null)
        {
            throw new FoosballDomainException("Set cannot be null.");
        }

        if (!set.Result.IsFinished())
        {
            throw new FoosballDomainException($"Any team must score {Goals.MaxValue} goals to consider set as finished.");
        }

        if (set.Result.IsEqual())
        {
            throw new FoosballDomainException($"Both teams cannot score {Goals.MaxValue} goals.");
        }

        Id = set.Id;
        Result = set.Result;
        WinnerTeamId = Result.TeamAGoals > Result.TeamBGoals ? TeamAId : TeamBId;
    }

    public static FinishedSet Finish(SetInProgress set)
        => new(set);
}
