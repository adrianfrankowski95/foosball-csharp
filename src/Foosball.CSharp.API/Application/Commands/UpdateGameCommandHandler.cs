
using Foosball.CSharp.API.Exceptions;
using Foosball.CSharp.Domain.Exceptions;
using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.API.Application.Commands;

public class UpdateGameCommandHandler : IUpdateGameCommandHandler
{
    private readonly IGameRepository _gameRepository;

    public UpdateGameCommandHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async Task<bool> HandleAsync(UpdateGameCommand command, CancellationToken cancellationToken = default)
    {
        var game = await _gameRepository.GetGameAsync(GameId.FromExisting(command.GameId), cancellationToken).ConfigureAwait(false);

        if (game is null)
        {
            throw new ObjectNotFoundException("game", command.GameId);
        }

        if (game is not GameInProgress gameInProgress)
        {
            throw new FoosballDomainException("Could not update a game that is not in progress anymore.");
        }

        var updatedGame = gameInProgress.UpdateOrBeginSet(
                Scores.Set(command.TeamAScore.Goals(), command.TeamBScore.Goals()),
                DateTime.UtcNow);

        return await _gameRepository.UpdateGameAsync(updatedGame, cancellationToken).ConfigureAwait(false);
    }
}
