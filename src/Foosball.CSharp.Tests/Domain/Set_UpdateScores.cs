
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Domain.TeamAggregateModel;
using Foosball.CSharp.Tests.Fixtures;

namespace Foosball.CSharp.Tests.Domain;

public class Set_UpdateScores : IClassFixture<ScoresFixture>, IClassFixture<TeamsFixture>
{
    private readonly ScoresFixture scoresFixture;

    public Set_UpdateScores(ScoresFixture scoresFixture)
    {
        this.scoresFixture = scoresFixture;
    }

    [Fact]
    public void Set_UpdatedWithNullScores_ThrowsDomainException()
    {
        var set = SetInProgress.Start(GameId.Create(), TeamId.Create(), TeamId.Create());

        Assert.Throws<FoosballDomainException>(() => set.UpdateScores(null!, DateTime.UtcNow));
    }

    [Fact]
    public void Set_UpdatedWithWonScores_IsFinished()
    {
        var set = SetInProgress.Start(GameId.Create(), TeamId.Create(), TeamId.Create());
        var finished = set.UpdateScores(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        Assert.True(finished is FinishedSet);
    }

    [Fact]
    public void Set_UpdatedWithScoresInProgress_IsInProgress()
    {
        var set = SetInProgress.Start(GameId.Create(), TeamId.Create(), TeamId.Create());
        var finished = set.UpdateScores(scoresFixture.ScoresInProgress, DateTime.UtcNow);
        Assert.True(finished is SetInProgress);
    }
}
