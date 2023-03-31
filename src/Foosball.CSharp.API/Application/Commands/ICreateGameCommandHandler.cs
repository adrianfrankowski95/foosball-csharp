using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.API.Application.Commands;

public interface ICreateGameCommandHandler : ICommandHandler<CreateGameCommand, GameId>
{

}
