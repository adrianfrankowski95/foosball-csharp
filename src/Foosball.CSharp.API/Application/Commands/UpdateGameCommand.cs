namespace Foosball.CSharp.API.Application.Commands;

public record UpdateGameCommand(Guid GameId, int TeamAScore, int TeamBScore) : ICommand;
