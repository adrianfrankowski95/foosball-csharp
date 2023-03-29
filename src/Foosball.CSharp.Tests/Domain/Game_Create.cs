using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.Tests.Domain;

public class Game_Create : IClassFixture<TeamsFixture>
{
    private readonly TeamsFixture teamsFixture;

    public Game_Create(TeamsFixture teamsFixture)
    {
        this.teamsFixture = teamsFixture;
    }

    [Fact]
    public void Create_GameCreated_HasOneSet()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        Assert.True(game.Sets.Count == 1);
    }

    [Fact]
    public void Create_GameCreatedWithTheSameTeam_ThrowsDomainException()
    {
        Assert.Throws<FoosballDomainException>(() => GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.FirstTeam, DateTime.UtcNow));
    }
}
