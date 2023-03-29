namespace Foosball.CSharp.API.Application.Commands;

public record CreateGameCommand(Guid TeamAId, Guid TeamBId);
