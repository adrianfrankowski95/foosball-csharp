using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;
using Foosball.CSharp.Tests.Fixtures;

namespace Foosball.CSharp.Tests.Domain;

public class Game_UpdateOrBeginSet : IClassFixture<ScoresFixture>, IClassFixture<TeamsFixture>
{
    private readonly ScoresFixture scoresFixture;
    private readonly TeamsFixture teamsFixture;

    public Game_UpdateOrBeginSet(ScoresFixture scoresFixture, TeamsFixture teamsFixture)
    {
        this.scoresFixture = scoresFixture;
        this.teamsFixture = teamsFixture;
    }

    [Fact]
    public void UpdateOrBeginSet_GameWithAllSetsFinished_ThrowsException()
    {
        var game = GetGameWithLastSet().UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        Assert.Throws<InvalidCastException>(() =>
        {
            ((GameInProgress)game).UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        });
    }

    [Fact]
    public void UpdateOrBeginSet_GameWithSetInProgressWithUpdatedScoresInProgress_NoSetsAdded()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        game.UpdateOrBeginSet(scoresFixture.ScoresInProgress, DateTime.UtcNow);

        Assert.True(game.Sets.Count == 1);
    }

    [Fact]
    public void UpdateOrBeginSet_GameWithSetInProgressWithUpdatedWonScores_NoSetsAdded()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        game.UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);

        Assert.True(game.Sets.Count == 1);
    }

    [Fact]
    public void UpdateOrBeginSet_GameWithSetInProgressWithUpdatedWonScores_LastSetIsFinished()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        game.UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);

        Assert.True(game.Sets[^1] is FinishedSet);
    }

    [Fact]
    public void UpdateOrBeginSet_GameWithLastSet_IsFinished()
    {
        var finishedGame = GetGameWithLastSet().UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        Assert.True(finishedGame is FinishedGame);
    }

    [Fact]
    public void UpdateOrBeginSet_FinishedGameInProgress_ThrowsDomainException()
    {
        var gameInProgress = GetGameWithLastSet();
        gameInProgress.UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.Now);

        Assert.Throws<FoosballDomainException>(() => gameInProgress.UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.Now));
    }

    [Fact]
    public void UpdateOrBeginSet_GameWithLastSet_CorrectTeamWon()
    {
        var finishedGame = GetGameWithLastSet().UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        Assert.True(((FinishedGame)finishedGame).WinnerTeamId.Equals(teamsFixture.FirstTeam.Id));
    }

    [Fact]
    public void UpdateOrBeginSet_GameWithWonRequiredNumberOfSets_IsFinished()
    {
        Game game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);

        int count = 1;
        while (count <= GameInProgress.SetsRequiredToWin)
        {
            game = ((GameInProgress)game).UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
            ++count;
        }

        Assert.True(game is FinishedGame);
    }

    [Fact]
    public void UpdateOrBeginSet_PreviousSetFinished_OneSetAdded()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        var updatedGame = game.UpdateOrBeginSet(scoresFixture.FirstTeamWonScores, DateTime.UtcNow);
        var setsCount = ((GameInProgress)updatedGame).Sets.Count;

        updatedGame = game.UpdateOrBeginSet(scoresFixture.ScoresInProgress, DateTime.UtcNow);

        Assert.True(((GameInProgress)updatedGame).Sets.Count == setsCount + 1);
    }

    [Fact]
    public void UpdateOrBeginSet_SetNotFinished_NoNewSetAdded()
    {
        var game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);
        var setsCount = game.Sets.Count;

        var updatedGame = game.UpdateOrBeginSet(scoresFixture.ScoresInProgress, DateTime.UtcNow);

        Assert.True(setsCount == ((GameInProgress)updatedGame).Sets.Count);
    }

    private GameInProgress GetGameWithLastSet()
    {
        Game game = GameInProgress.Create(teamsFixture.FirstTeam, teamsFixture.SecondTeam, DateTime.UtcNow);

        int count = 1;
        while (count < GameInProgress.MaxSets)
        {
            var scores = count % 2 == 1 ? scoresFixture.FirstTeamWonScores : scoresFixture.SecondTeamWonScores;

            game = ((GameInProgress)game).UpdateOrBeginSet(scores, DateTime.UtcNow);
            ++count;
        }

        return (GameInProgress)game;
    }
}
