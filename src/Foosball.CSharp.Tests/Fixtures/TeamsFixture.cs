using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Tests.Fixtures;

public class TeamsFixture
{
    public OnePlayerTeam FirstTeam { get; }
    public OnePlayerTeam SecondTeam { get; }

    public TeamsFixture()
    {
        FirstTeam = new OnePlayerTeam("Team1", PlayerId.Create());
        SecondTeam = new OnePlayerTeam("Team2", PlayerId.Create());
    }
}