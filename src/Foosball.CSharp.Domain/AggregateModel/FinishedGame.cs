using Foosball.CSharp.Domain.Events;
using Foosball.CSharp.Domain.Exceptions;

namespace Foosball.CSharp.Domain.AggregateModel;

public class FinishedGame : Game
{
    private readonly List<FinishedSet> _sets = new();
    public IReadOnlyList<FinishedSet> Sets => _sets;
    public TeamId WinnerTeamId { get; }

    // Required by EF :-( Private, though :-)
    private FinishedGame() { }

    private FinishedGame(GameInProgress game)
    {
        if (game is null)
        {
            throw new FoosballDomainException("Game cannot be null.");
        }

        if (game.Sets.Any(s => s is not FinishedSet))
        {
            throw new FoosballDomainException("All sets must be finished to consider the game finished");
        }

        Id = game.Id;
        StartedAt = game.StartedAt;

        AddSets(game.Sets.Cast<FinishedSet>());

        WinnerTeamId = GetWinner();
        
        var lastSet = Sets[^1];
        AddDomainEvent(new GameFinishedDomainEvent(Id, lastSet.TeamAId, lastSet.TeamBId, WinnerTeamId, Sets));
    }

    public static FinishedGame Finish(GameInProgress game)
        => new(game);

    private void AddSets(IEnumerable<FinishedSet> sets)
    {
        _sets.AddRange(sets);
    }

    private Dictionary<TeamId, int> GetWins()
        => _sets.GroupBy(s => s.WinnerTeamId).ToDictionary(s => s.Key, s => s.Count());

    private TeamId GetWinner() => GetWins().First(w => w.Value == GetWins().Values.Max()).Key;
}
