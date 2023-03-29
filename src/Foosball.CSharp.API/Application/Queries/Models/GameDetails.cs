
namespace Foosball.CSharp.API.Application.Queries.Models;

public record GameDetails
{
    public Guid GameId { get; init; }
    public string Status { get; init; }
    public Guid TeamAId { get; init; }
    public string TeamAName { get; init; }
    public Guid TeamBId { get; init; }
    public string TeamBName { get; init; }
    public Guid? WinnerTeamId { get; init; }
    public string? WinnerTeamName { get; init; }
    public DateTime StartedAt { get; init; }
    public List<SetDetails> Sets { get; init; } = new();
}


public record SetDetails(
    Guid SetId,
    string Status,
    int TeamAScore,
    int TeamBScore,
    DateTime? FinishedAt,
    Guid? WinnerTeamId,
    string? WinnerTeamName);