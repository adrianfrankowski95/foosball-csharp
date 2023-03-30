
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Foosball.CSharp.Tests.Fixtures;

namespace Foosball.CSharp.Tests.Domain;

public class Set_Create : IClassFixture<ScoresFixture>, IClassFixture<TeamsFixture>
{
    private readonly ScoresFixture scoresFixture;

    public Set_Create(ScoresFixture scoresFixture)
    {
        this.scoresFixture = scoresFixture;
    }

    [Fact]
    public void Create_CreatedWithWonScores_IsFinished()
    {
        var set = SetInProgress.WithScores(GameId.Create(), TeamId.Create(), TeamId.Create(), scoresFixture.FirstTeamWonScores, DateTime.Now);
        Assert.True(set is FinishedSet);
    }

    [Fact]
    public void Create_CreatedWithScoresInProgress_IsInProgress()
    {
        var set = SetInProgress.WithScores(GameId.Create(), TeamId.Create(), TeamId.Create(), scoresFixture.ScoresInProgress, DateTime.Now);
        Assert.True(set is SetInProgress);
    }

    [Fact]
    public void Create_Started_IsInProgress()
    {
        var set = SetInProgress.Start(GameId.Create(), TeamId.Create(), TeamId.Create());
        Assert.True(set is SetInProgress);
    }
}
