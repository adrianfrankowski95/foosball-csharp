

namespace Foosball.CSharp.API.Application.Queries.Models;

public record GameOverview(
    Guid GameId,
    string Status,
    long CurrentSet,
    Guid TeamAId,
    string TeamAName,
    int TeamACurrentScore,
    Guid TeamBId,
    string TeamBName,
    int TeamBCurrentScore,
    Guid? WinnerTeamId,
    string? WinnerTeamName,
    DateTime StartedAt);
