

namespace Foosball.CSharp.API.Application.Queries.Models;

public record GameOverview(
    Guid GameId,
    string Status,
    DateTime StartedAt,
    Guid? WinnerTeamId,
    Guid TeamAId,
    Guid TeamBId,
    string TeamAName,
    string TeamBName,
    string? WinnerTeamName,
    long CurrentSet,
    int TeamACurrentScore,
    int TeamBCurrentScore);
