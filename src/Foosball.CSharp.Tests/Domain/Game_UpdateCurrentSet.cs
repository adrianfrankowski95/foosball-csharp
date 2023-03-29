using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.Tests.Domain;

public class Game_UpdateCurrentSet : IClassFixture<ScoresFixture>, IClassFixture<TeamsFixture>
{
    private readonly ScoresFixture scoresFixture;
    private readonly TeamsFixture teamsFixture;

    public Game_UpdateCurrentSet(ScoresFixture scoresFixture, TeamsFixture teamsFixture)
    {
        this.scoresFixture = scoresFixture;
        this.teamsFixture = teamsFixture;
    }

    [Fact]
    public void UpdateCurrentSet_GameWithAllSetsFinished_ThrowsException()
    {
        var game = GetGameWithLastSet().UpdateCurrentSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        Assert.Throws<InvalidCastException>(() =>
        {
            ((GameInProgress)game).UpdateCurrentSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        });
    }

    [Fact]
    public void FinishCurrentSet_GameWithLastSet_GameIsFinished()
    {
        var finishedGame = GetGameWithLastSet().UpdateCurrentSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        Assert.True(finishedGame is FinishedGame);
    }

    [Fact]
    public void UpdateCurrentSet_GameWithWonRequiredNumberOfSets_IsFinished()
    {
        Game game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);

        int count = 1;
        while (count <= GameInProgress.SetsRequiredToWin)
        {
            game = ((GameInProgress)game).UpdateCurrentSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
            ++count;
        }

        Assert.True(game is FinishedGame);
    }

    [Fact]
    public void UpdateCurrentSet_PreviousSetFinished_OneSetAdded()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        var updatedGame = game.UpdateCurrentSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        var setsCount = ((GameInProgress)updatedGame).Sets.Count;

        updatedGame = game.UpdateCurrentSet(scoresFixture.ScoresInProgress, DateTime.UtcNow);

        Assert.True(((GameInProgress)updatedGame).Sets.Count == setsCount + 1);
    }

    [Fact]
    public void UpdateCurrentSet_SetNotFinished_NoNewSetAdded()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        var setsCount = game.Sets.Count;

        var updatedGame = game.UpdateCurrentSet(scoresFixture.ScoresInProgress, DateTime.UtcNow);

        Assert.True(setsCount == ((GameInProgress)updatedGame).Sets.Count);
    }

    private GameInProgress GetGameWithLastSet()
    {
        Game game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);

        int count = 1;
        while (count < GameInProgress.MaxSets)
        {
            var scores = count % 2 == 1 ? scoresFixture.FirstTeamWonScores : scoresFixture.SecondTeamWonScores;

            game = ((GameInProgress)game).UpdateCurrentSet(scores, DateTime.UtcNow);
            ++count;
        }

        return (GameInProgress)game;
    }
}
