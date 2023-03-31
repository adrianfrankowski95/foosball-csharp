using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Domain.GameAggregateModel;

public class SetInProgress : Set
{
    private SetInProgress(GameId gameId, TeamId teamAId, TeamId teamBId) : base(gameId, teamAId, teamBId)
    {
        Id = SetId.Create();
        Scores = Scores.Empty();
    }

    public static SetInProgress Start(GameId gameId, TeamId teamAId, TeamId teamBId)
        => new(gameId, teamAId, teamBId);

    public static Set WithScores(GameId gameId, TeamId teamAId, TeamId teamBId, Scores scores, DateTime now)
    {
        var set = new SetInProgress(gameId, teamAId, teamBId);
        return set.UpdateScores(scores, now);
    }

    public Set UpdateScores(Scores scores, DateTime now)
    {
        if (scores is null)
        {
            throw new FoosballDomainException("Scores cannot be null.");
        }

        if (Scores.HaveWinner())
        {
            throw new FoosballDomainException("This set is already finished.");
        }

        if (scores.AnyScoreLowerThan(Scores))
        {
            throw new FoosballDomainException("New number of scored goals cannot be lower than the previous one.");
        }

        Scores = scores;
        return Scores.HaveWinner() ? FinishedSet.Finish(this, now) : this;
    }
}
