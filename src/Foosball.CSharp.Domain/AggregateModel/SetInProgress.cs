using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class SetInProgress : Set
{
    private SetInProgress(GameId gameId, TeamId teamAId, TeamId teamBId) : base(gameId, teamAId, teamBId)
    {
        Id = SetId.Create();
        Result = SetResult.Empty();
    }

    public static SetInProgress Start(GameId gameId, TeamId teamAId, TeamId teamBId)
        => new(gameId, teamAId, teamBId);

    public Set UpdateResult(SetResult newResult)
    {
        if (newResult is null)
        {
            throw new FoosballDomainException("Result cannot be null.");
        }

        if (newResult.AnyLowerThan(Result))
        {
            throw new FoosballDomainException("New number of scored goals cannot be lower than the previous one.");
        }

        Result = newResult;

        return Result.IsFinished() ? FinishedSet.Finish(this) : this;
    }
}
