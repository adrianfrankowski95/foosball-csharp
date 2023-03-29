using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.Tests.Domain;

public class ScoresFixture
{
    public Scores FirstTeamWonScores { get; }
    public Scores SecondTeamWonScores { get; }
    public Scores ScoresInProgress { get; }

    public ScoresFixture()
    {
        FirstTeamWonScores = Scores.Set(Goals.ToWin.Goals(), 0.Goals());
        ScoresInProgress = Scores.Set(5.Goals(), 8.Goals());
        SecondTeamWonScores = Scores.Set(0.Goals(), Goals.ToWin.Goals());
    }
}