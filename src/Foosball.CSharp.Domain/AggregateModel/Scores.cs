using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.SeedWork;

namespace Foosball.CSharp.Domain.AggregateModel;

public class Scores : ValueObject
{
    public Goals TeamAScore { get; }
    public Goals TeamBScore { get; }

    public Scores(Goals teamAScore, Goals teamBScore)
    {
        if (teamAScore is null || teamBScore is null)
        {
            throw new FoosballDomainException("Set scores cannot be null.");
        }

        TeamAScore = teamAScore;
        TeamBScore = teamBScore;
    }

    public bool HaveWinner() => TeamAScore == Goals.ToWin || TeamBScore == Goals.ToWin;
    public bool IsDraw() => TeamAScore == TeamBScore;
    public bool AnyScoreLowerThan(Scores other) => TeamAScore < other.TeamAScore || TeamBScore < other.TeamBScore;

    public static Scores Empty() => new(0.Goals(), 0.Goals());

    protected override IEnumerable<object> GetEqualityAttributes()
    {
        yield return TeamAScore;
        yield return TeamBScore;
    }
}
